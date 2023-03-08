using TvMazeScraper.Domain.Entities.Base;
using DomainEntities = TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Common.Interface.Persistence
{
    public interface IShowRepository<T> : IRepositoryBase<T> where T : EntityBase 
    {
        IEnumerable<DomainEntities.Show> GetPaginatedShows(int pageNumber, int pageSize);
    }
}
