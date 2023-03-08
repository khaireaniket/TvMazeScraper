namespace TvMazeScraper.Application.Shows.Queries.DTOs
{
    public class Cast
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public DateOnly Birthday { get; set; } = default!;
    }
}
