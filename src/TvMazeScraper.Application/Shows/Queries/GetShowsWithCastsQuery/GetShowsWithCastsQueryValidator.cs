using FluentValidation;

namespace TvMazeScraper.Application.Shows.Queries.GetShowsWithCastsQuery
{
    public class GetShowsWithCastsQueryValidator : AbstractValidator<GetShowsWithCastsQuery>
    {
        public GetShowsWithCastsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(250);
        }
    }
}
