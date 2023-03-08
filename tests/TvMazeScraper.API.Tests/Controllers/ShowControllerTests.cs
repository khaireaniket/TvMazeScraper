using Microsoft.AspNetCore.Mvc;
using Moq;
using TvMazeScraper.Application.Shows.Queries.GetShowsWithCastsQuery;
using Xunit;
using ShowsDTOs = TvMazeScraper.Application.Shows.Queries.DTOs;

namespace TvMazeScraper.API.Tests.Controllers
{
    public sealed class ShowControllerTests : ShowControllerTestsBase
    {
        [Fact]
        public async Task GetShowsWithCasts_ShouldReturnPaginatedShowsWithCasts_WhenPageRangeProvided()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetShowsWithCastsQuery>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(_listOfShows));

            // Act
            var result = await _showController.GetShowsWithCasts(new GetShowsWithCastsQuery(1, 1));

            // Assert
            Assert.NotNull(result);

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.NotNull(okObjectResult.Value);

            var allShows = (List<ShowsDTOs.Show>)okObjectResult.Value;

            Assert.NotNull(allShows);
            Assert.Equal(_listOfShows.ToList().Count, allShows.Count);
            Assert.Equal(_listOfShows.Single().Id, allShows.Single().Id);
        }

        [Fact]
        public async Task GetShowsWithCasts_ShouldReturnEmptyResponse_WhenNoDataAvailable()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<GetShowsWithCastsQuery>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.FromResult(_emptyListOfShows));

            // Act
            var result = await _showController.GetShowsWithCasts(new GetShowsWithCastsQuery(1, 1));

            // Assert
            Assert.NotNull(result);

            var noContentObjectResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentObjectResult.StatusCode);
        }
    }
}
