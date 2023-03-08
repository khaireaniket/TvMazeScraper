using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Application.Shows.Queries.GetShowsWithCastsQuery;

namespace TvMazeScraper.API.Controllers
{
    /// <summary>
    /// Controller to delegate calls to the appropriate command and query handlers
    /// </summary>
    [Route("api/show")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="mediator"></param>
        public ShowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Endpoint will be used to fetch all the shows with casts
        /// </summary>
        /// <param name="query">Pagination parameters</param>
        /// <returns>List Of shows with casts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetShowsWithCasts([FromQuery] GetShowsWithCastsQuery query)
        {
            var listOfShows = await _mediator.Send(query);

            if (listOfShows is null || !listOfShows.Any())
                return NoContent();

            return Ok(listOfShows);
        }
    }
}
