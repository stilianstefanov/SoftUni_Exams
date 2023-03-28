namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Shell
    {
        public Shell()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }
       
        public double ShellWeight { get; set; }

        [MaxLength(ValidationConstants.ShellCaliberMaxLength)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; } = null!;
    }
}
