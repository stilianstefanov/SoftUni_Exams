namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Despatcher
    {
        public Despatcher()
        {
            Trucks = new HashSet<Truck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.DispatcherNameMaxLength)]
        public string Name { get; set; } = null!;

        public string Position { get; set; } = null!;

        public virtual ICollection<Truck> Trucks { get; set; }
    }
}