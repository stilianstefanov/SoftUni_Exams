namespace TeisterMask.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.ProjectNameMaxLength)]
        public string Name { get; set; } = null!;

        public DateTime OpenDate { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = null!;
    }
}
