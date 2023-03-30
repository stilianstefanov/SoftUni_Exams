namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Developer
    {
        public Developer()
        {
            Games = new HashSet<Game>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; } = null!;
    }
}
