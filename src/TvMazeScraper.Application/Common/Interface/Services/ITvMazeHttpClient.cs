namespace TvMazeScraper.Application.Common.Interface.Services
{
    public interface ITvMazeHttpClient
    {
        Task<string?> FetchShowsByPageNumber(int pageNumber);

        Task<string?> FetchCastsByShow(int showId);
    }
}
