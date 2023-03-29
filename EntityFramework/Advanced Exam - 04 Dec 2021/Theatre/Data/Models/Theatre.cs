namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Theatre
    {
        public Theatre()
        {
            Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.TheatreNameMaxLength)]
        public string Name { get; set; } = null!;

        public sbyte NumberOfHalls { get; set; }

        [MaxLength(ValidationConstants.TheatreDirectorMaxLength)]
        public string Director { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = null!;
    }
}
