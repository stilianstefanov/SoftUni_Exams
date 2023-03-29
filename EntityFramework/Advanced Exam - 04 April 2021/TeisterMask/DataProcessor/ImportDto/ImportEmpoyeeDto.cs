namespace TeisterMask.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportEmpoyeeDto
    {
        [JsonProperty("Username")]
        [MinLength(ValidationConstants.EmployeeUserNameMinLength)]
        [MaxLength(ValidationConstants.EmployeeUserNameMaxLength)]
        [RegularExpression(ValidationConstants.EmployeeUserNameRegex)]
        public string Username { get; set; } = null!;

        [JsonProperty("Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [JsonProperty("Phone")]
        [RegularExpression(ValidationConstants.EmployeePhoneRegex)]
        public string Phone { get; set; } = null!;

        [JsonProperty("Tasks")]
        public int[] Tasks { get; set; } = null!;
    }
}
