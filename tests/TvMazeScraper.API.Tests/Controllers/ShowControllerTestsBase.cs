using MediatR;
using Moq;
using TvMazeScraper.API.Controllers;
using ShowsDTOs = TvMazeScraper.Application.Shows.Queries.DTOs;

namespace TvMazeScraper.API.Tests.Controllers
{
    public class ShowControllerTestsBase
    {
        protected readonly Mock<IMediator> _mediator;
        protected readonly ShowController _showController;

        protected ShowsDTOs.Show _show;
        protected IEnumerable<ShowsDTOs.Show> _listOfShows;
        protected IEnumerable<ShowsDTOs.Show> _emptyListOfShows;

        public ShowControllerTestsBase()
        {
            _mediator = new Mock<IMediator>();
            _showController = new ShowController(_mediator.Object);

            _show = new ShowsDTOs.Show
            {
                Id = 1
            };

            _listOfShows = new List<ShowsDTOs.Show> { _show };
            _emptyListOfShows = new List<ShowsDTOs.Show>();
        }
    }
}
