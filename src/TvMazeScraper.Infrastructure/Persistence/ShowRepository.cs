using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Application.Common.Interface.Persistence;
using DomainEntities = TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Infrastructure.Persistence
{
    public class ShowRepository : BaseRepository<DomainEntities.Show>, IShowRepository<DomainEntities.Show>
    {
        public ShowRepository(TvMazeScraperDbContext tvMazeScraperDbContext) : base(tvMazeScraperDbContext) { }

        public IEnumerable<DomainEntities.Show> GetPaginatedShows(int pageNumber, int pageSize)
        {
            return _tvMazeScraperDbContext.Shows
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Include(i => i.Casts)
                            .Select(s => new DomainEntities.Show(s.Id, s.Name)
                            {
                                Casts = s.Casts.OrderByDescending(o => o.Birthday).ToList()
                            })
                            .AsEnumerable();
        }
    }
}
