using AutoMapper;
using MediatR;
using TvMazeScraper.Application.Common.Interface.Persistence;
using DomainEntities = TvMazeScraper.Domain.Entities;
using ShowsDTOs = TvMazeScraper.Application.Shows.Queries.DTOs;

namespace TvMazeScraper.Application.Shows.Queries.GetShowsWithCastsQuery
{
    public record GetShowsWithCastsQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<ShowsDTOs.Show>>;

    public class GetShowsWithCastsQueryHandler : IRequestHandler<GetShowsWithCastsQuery, IEnumerable<ShowsDTOs.Show>>
    {
        private readonly IShowRepository<DomainEntities.Show> _showRepository;
        private readonly IMapper _mapper;

        public GetShowsWithCastsQueryHandler(IShowRepository<DomainEntities.Show> showRepository, IMapper mapper)
        {
            _showRepository = showRepository;
            _mapper = mapper;
        }

        public Task<IEnumerable<ShowsDTOs.Show>> Handle(GetShowsWithCastsQuery request, CancellationToken cancellationToken)
        {
            var domainShows = _showRepository.GetPaginatedShows(request.PageNumber, request.PageSize);
            return Task.FromResult(_mapper.Map<IEnumerable<ShowsDTOs.Show>>(domainShows));
        }
    }
}
