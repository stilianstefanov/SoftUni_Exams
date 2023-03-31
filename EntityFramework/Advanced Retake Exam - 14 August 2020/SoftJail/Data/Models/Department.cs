namespace SoftJail.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        public Department()
        {
            Cells = new HashSet<Cell>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.DepartmentNameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Cell> Cells { get; set; } = null!;
    }
}
