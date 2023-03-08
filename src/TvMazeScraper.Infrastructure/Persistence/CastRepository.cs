using TvMazeScraper.Application.Common.Interface.Persistence;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Infrastructure.Persistence
{
    public class CastRepository : BaseRepository<Cast>, ICastRepository<Cast>
    {
        public CastRepository(TvMazeScraperDbContext tvMazeScraperDbContext) : base(tvMazeScraperDbContext) { }
    }
}
