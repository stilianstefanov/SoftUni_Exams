namespace TeisterMask.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        public Employee()
        {
            EmployeesTasks = new HashSet<EmployeeTask>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.EmployeeUserNameMaxLength)]
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; } = null!;
    }
}
