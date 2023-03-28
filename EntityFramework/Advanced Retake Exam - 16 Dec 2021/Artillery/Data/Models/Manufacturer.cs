namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Manufacturer
    {
        public Manufacturer()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.ManNameMaxLength)]        
        public string ManufacturerName { get; set; } = null!;

        [MaxLength(ValidationConstants.ManFoundedMaxLength)]
        public string Founded { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; } = null!;
    }
}
