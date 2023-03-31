namespace SoftJail.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportPrisonerDto
    {
        [JsonProperty("FullName")]
        [MaxLength(ValidationConstants.PrisonerFullNameMaxLength)]
        [MinLength(ValidationConstants.PrisonerFullNameMinLength)]
        [Required]
        public string? FullName { get; set; }

        [JsonProperty("Nickname")]
        [RegularExpression(ValidationConstants.PrisonerNickNameRegex)]
        [Required]
        public string? Nickname { get; set; }

        [JsonProperty("Age")]
        [Range(ValidationConstants.PrisonerAgeMinValue, ValidationConstants.PrisonerAgeMaxValue)]
        [Required]
        public int? Age { get; set; }

        [JsonProperty("IncarcerationDate")]
        [Required]
        public string? IncarcerationDate { get; set; }

        [JsonProperty("ReleaseDate")]
        public string? ReleaseDate { get; set; }

        [JsonProperty("Bail")]
        public decimal? Bail { get; set; }

        [JsonProperty("CellId")]
        public int? CellId { get; set; }

        [JsonProperty("Mails")]
        public ImportMailDto[] Mails { get; set; } = null!;
    }
}
