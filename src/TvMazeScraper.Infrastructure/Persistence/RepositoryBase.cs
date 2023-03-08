using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TvMazeScraper.Application.Common.Interface.Persistence;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Domain.Entities.Base;

namespace TvMazeScraper.Infrastructure.Persistence
{
    public abstract class BaseRepository<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly TvMazeScraperDbContext _tvMazeScraperDbContext;

        protected BaseRepository(TvMazeScraperDbContext tvMazeScraperDbContext)
        {
            _tvMazeScraperDbContext = tvMazeScraperDbContext;
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            _tvMazeScraperDbContext.Set<T>().Add(entity);
            return await _tvMazeScraperDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            _tvMazeScraperDbContext.Set<T>().AddRange(entities);
            return await _tvMazeScraperDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _tvMazeScraperDbContext.Set<T>().AnyAsync(predicate);
        }

        public virtual async Task<List<Show>> GetAsync(int id)
        {
            return await _tvMazeScraperDbContext.Shows.Where(s => s.Id == id).Include(s => s.Casts).ToListAsync();
        }

        public virtual async Task<int> CountAsync()
        {
            return await _tvMazeScraperDbContext.Set<T>().CountAsync();
        }

        public virtual void DetachAsync(T entity)
        {
            _tvMazeScraperDbContext.Set<T>().Entry(entity).DetectChanges();
            _tvMazeScraperDbContext.ChangeTracker.Clear();
        }
    }
}
