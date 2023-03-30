namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public Tag()
        {
            GameTags = new HashSet<GameTag>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<GameTag> GameTags { get; set; } = null!;
    }
}
