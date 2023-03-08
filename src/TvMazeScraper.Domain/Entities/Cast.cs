using TvMazeScraper.Domain.Entities.Base;

namespace TvMazeScraper.Domain.Entities
{
    public class Cast : EntityBase
    {
        public Cast(int id, string name, DateTime birthday, int showId)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
            ShowId = showId;
        }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public virtual int ShowId { get; set; }

        public virtual Show Show { get; set; } = default!;
    }
}
