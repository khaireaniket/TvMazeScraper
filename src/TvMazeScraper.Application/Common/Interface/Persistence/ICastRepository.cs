using TvMazeScraper.Domain.Entities.Base;

namespace TvMazeScraper.Application.Common.Interface.Persistence
{
    public interface ICastRepository<T> : IRepositoryBase<T> where T : EntityBase { }
}
