namespace TeisterMask.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class EmployeeTask
    {
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }

        public Task Task { get; set; } = null!;
    }
}
