namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Coach
    {
        public Coach()
        {
            Footballers = new HashSet<Footballer>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.CoachNameMaxLength)]
        public string Name { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public ICollection<Footballer> Footballers { get; set; } = null!;
    }
}
