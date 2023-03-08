using AutoMapper;
using DomainEntities = TvMazeScraper.Domain.Entities;
using ShowsDTOs = TvMazeScraper.Application.Shows.Queries.DTOs;

namespace TvMazeScraper.Application.Common.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<DomainEntities.Cast, ShowsDTOs.Cast>()
               .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Birthday)))
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<DomainEntities.Show, ShowsDTOs.Show>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
