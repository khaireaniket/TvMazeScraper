using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Application.Common.Interface.Persistence;
using TvMazeScraper.Application.Common.Interface.Services;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Infrastructure.Persistence;
using TvMazeScraper.Infrastructure.Services;

namespace TvMazeScraper.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddDbContext<TvMazeScraperDbContext>(options => options.UseInMemoryDatabase(databaseName: "TvMazeScraperDb")
                                                                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Singleton);

            services.AddTransient<IShowRepository<Show>, ShowRepository>();
            services.AddTransient<ICastRepository<Cast>, CastRepository>();

            services.AddHttpClient<ITvMazeHttpClient, TvMazeHttpClient>(client => {
                client.BaseAddress = new Uri("https://api.tvmaze.com");
            });

            return services;
        }
    }
}
