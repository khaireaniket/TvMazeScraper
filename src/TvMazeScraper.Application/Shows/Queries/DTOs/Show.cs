namespace TvMazeScraper.Application.Shows.Queries.DTOs
{
    public class Show
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public ICollection<Cast> Casts { get; set; } = new List<Cast>();

    }
}
