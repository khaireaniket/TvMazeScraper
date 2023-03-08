using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TvMazeScraper.Application.Common.Interface.Persistence;
using TvMazeScraper.Application.Common.Interface.Services;
using DomainEntities = TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Infrastructure.Services
{
    public class Scraper : BackgroundService, IScraper
    {
        private readonly ILogger<Scraper> _logger;
        private readonly ITvMazeHttpClient _tvMazeHttpClient;
        private readonly IServiceProvider _services;
        private readonly IShowRepository<DomainEntities.Show> _showRepository;
        private readonly ICastRepository<DomainEntities.Cast> _castRepository;

        public Scraper(ILogger<Scraper> logger, ITvMazeHttpClient tvMazeHttpClient, IServiceProvider services,
                       IShowRepository<DomainEntities.Show> showRepository, ICastRepository<DomainEntities.Cast> castRepository)
        {
            _logger = logger;
            _tvMazeHttpClient = tvMazeHttpClient;
            _services = services;
            _showRepository = showRepository;
            _castRepository = castRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scraping of TVMaze API has started");

            int showId = 0;

            using (var scope = _services.CreateScope())
            {
                var lastPageScraped = await EvaluateLastPageScraped();

                while (true)
                {
                    try
                    {
                        var showsResponse = await _tvMazeHttpClient.FetchShowsByPageNumber(lastPageScraped);

                        if (showsResponse is not null)
                        {
                            var showsArray = JsonConvert.DeserializeObject<JArray>(showsResponse);

                            if (showsArray is not null && showsArray is JArray && showsArray.Any())
                            {
                                foreach (var show in showsArray)
                                {
                                    showId = show.Value<int>("id");
                                    if (showId > 0)
                                    {
                                        var castsResponse = await _tvMazeHttpClient.FetchCastsByShow(showId);

                                        if (castsResponse is not null)
                                        {
                                            var castsArray = JsonConvert.DeserializeObject<JArray>(castsResponse);

                                            if (castsArray is not null && castsArray is JArray && castsArray.Any())
                                            {
                                                var castsInserted = false;
                                                var showInserted = false;
                                                DomainEntities.Show entityShow = null!;
                                                try
                                                {
                                                    (entityShow, showInserted) = await InsertShow(show);

                                                    if (showInserted)
                                                    {
                                                        castsInserted = await InsertCasts(entityShow.Id, castsArray, entityShow);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logger.LogError(ex.Message, ex);
                                                    Console.WriteLine(ex);

                                                    if (!showInserted)
                                                    {
                                                        // Implement retry mechanism
                                                        continue;
                                                    }
                                                    if (!castsInserted)
                                                    {
                                                        // Implement retry mechanism
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            lastPageScraped++;
                        }
                        else
                        {
                            _logger.LogInformation("Scraping of TVMaze API has completed successfully");
                            break;
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        _logger.LogError(ex.Message, ex);

                        // Implement retry mechanism
                        continue;
                    }
                }
            }
        }

        private async Task<int> EvaluateLastPageScraped()
        {
            var lastPageScraped = 0;
            var numberOfShowsInserted = await _showRepository.CountAsync();

            if (numberOfShowsInserted > 0)
            {
                lastPageScraped = Convert.ToInt32(Math.Round(numberOfShowsInserted / 250d, MidpointRounding.ToZero));
            }

            return lastPageScraped;
        }

        private async Task<(DomainEntities.Show, bool)> InsertShow(JToken show)
        {
            var showInserted = false;
            var domainShow = new DomainEntities.Show(
                id: show.SelectToken("id", false)!.Value<int?>() ?? 0,
                name: show.SelectToken("name", false)!.Value<string>() ?? string.Empty);

            if (!await _showRepository.AnyAsync(a => a.Id == domainShow.Id))
            {
                try
                {
                    await _showRepository.AddAsync(domainShow);
                    showInserted = true;
                }
                catch (Exception ex)
                {
                    _showRepository.DetachAsync(domainShow);
                }
            }
            else
            {
                var tem = await _showRepository.GetAsync(domainShow.Id);
            }

            return (domainShow, showInserted);
        }

        private async Task InsertCast(JToken cast, DomainEntities.Show show)
        {
            var domainCast = new DomainEntities.Cast(
                                    id: cast.SelectToken("person.id", false)!.Value<int?>() ?? 0,
                                    name: cast.SelectToken("person.name", false)!.Value<string>() ?? string.Empty,
                                    birthday: cast.SelectToken("person.birthday", false)!.Value<DateTime?>() ?? DateTime.MinValue,
                                    showId: show.Id)
            {
                Show = show
            };

            if (!await _castRepository.AnyAsync(a => a.Id == domainCast.Id))
            {
                await _castRepository.AddAsync(domainCast);
            }
        }

        private async Task<bool> InsertCasts(int showId, JArray castsArray, DomainEntities.Show show)
        {
            var uniqueCasts = castsArray.GroupBy(c => c.SelectToken("person.id")).ToArray();

            var listOfDomainCasts = uniqueCasts.Select(c => new DomainEntities.Cast(
                                                        id: c.First().SelectToken("person.id", false)!.Value<int?>() ?? 0,
                                                        name: c.First().SelectToken("person.name", false)!.Value<string>() ?? string.Empty,
                                                        birthday: c.First().SelectToken("person.birthday", false)!.Value<DateTime?>() ?? DateTime.MinValue,
                                                        showId: show.Id)
            {
                Show = show
            });

            return await _castRepository.AddRangeAsync(listOfDomainCasts);
        }
    }
}