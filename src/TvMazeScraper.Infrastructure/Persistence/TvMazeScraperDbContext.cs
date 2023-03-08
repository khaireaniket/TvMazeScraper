using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Infrastructure.Persistence
{
    public class TvMazeScraperDbContext : DbContext
    {
        public TvMazeScraperDbContext(DbContextOptions<TvMazeScraperDbContext> options) : base(options) { }

        public DbSet<Show> Shows { get; set; }

        public DbSet<Cast> Casts { get; set; }
    }
}
