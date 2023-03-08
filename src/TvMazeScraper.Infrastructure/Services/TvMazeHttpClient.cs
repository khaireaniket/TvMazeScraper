using System.Net;
using TvMazeScraper.Application.Common.Interface.Services;

namespace TvMazeScraper.Infrastructure.Services
{
    public class TvMazeHttpClient : ITvMazeHttpClient
    {
        private readonly HttpClient _httpClient;

        public TvMazeHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> FetchShowsByPageNumber(int pageNumber)
        {
            var response = await _httpClient.GetAsync($"/shows?page={pageNumber}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> FetchCastsByShow(int showId)
        {
            var response = await _httpClient.GetAsync($"/shows/{showId}/cast");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
