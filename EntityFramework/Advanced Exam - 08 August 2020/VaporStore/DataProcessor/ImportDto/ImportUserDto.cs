namespace VaporStore.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportUserDto
    {
        [JsonProperty("Username")]
        [MinLength(ValidationConstants.UserUserNameMinLength)]
        [MaxLength(ValidationConstants.UserUserNameMaxLength)]
        [Required]
        public string? Username { get; set; }

        [JsonProperty("FullName")]
        [RegularExpression(ValidationConstants.UserFullNameRegex)]
        [Required]
        public string? FullName { get; set; }

        [JsonProperty("Email")]
        [Required]
        public string? Email { get; set; }

        [Range(ValidationConstants.UserAgeMinValue, ValidationConstants.UserAgeMaxValue)]
        [JsonProperty("Age")]
        [Required]
        public int Age { get; set; }

        [JsonProperty("Cards")]
        public ImportCardDto[]? Cards { get; set; }
    }
}
