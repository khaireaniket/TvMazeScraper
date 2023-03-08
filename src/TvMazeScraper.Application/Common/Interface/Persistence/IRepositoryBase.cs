using System.Linq.Expressions;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Domain.Entities.Base;

namespace TvMazeScraper.Application.Common.Interface.Persistence
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<bool> AddAsync(T entity);

        Task<bool> AddRangeAsync(IEnumerable<T> entities);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<List<Show>> GetAsync(int id);

        Task<int> CountAsync();

        void DetachAsync(T entity);
    }
}
