using TvMazeScraper.Domain.Entities.Base;

namespace TvMazeScraper.Domain.Entities
{
    public class Show : EntityBase
    {
        public Show(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }

        public virtual List<Cast> Casts { get; set; } = default!;
    }
}
