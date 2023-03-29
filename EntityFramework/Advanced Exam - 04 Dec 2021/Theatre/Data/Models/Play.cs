namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Enums;
 

    public class Play
    {
        public Play()
        {
            Casts = new HashSet<Cast>();
            Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.PlayeTitleMaxValue)]
        public string Title { get; set; } = null!;

        public TimeSpan Duration { get; set; }

        public float Rating { get; set; }

        public Genre Genre { get; set; }

        [MaxLength(ValidationConstants.PlayeDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(ValidationConstants.PlayScreenWriterMaxLength)]
        public string Screenwriter { get; set; } = null!;

        public virtual ICollection<Cast> Casts { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = null!;
    }
}
