namespace SoftJail.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Prisoner
    {
        public Prisoner()
        {
            Mails = new HashSet<Mail>();
            PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.PrisonerFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        public string Nickname { get; set; } = null!;

        public int Age { get; set; }

        public DateTime IncarcerationDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal? Bail { get; set; }

        [ForeignKey(nameof(Cell))]
        public int? CellId { get; set; }

        public Cell? Cell { get; set; }

        public virtual ICollection<Mail> Mails { get; set; } = null!;

        public virtual ICollection<OfficerPrisoner> PrisonerOfficers { get; set; } = null!;
    }
}
