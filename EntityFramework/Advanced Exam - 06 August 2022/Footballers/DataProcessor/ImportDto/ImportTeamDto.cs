using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [JsonProperty("Name")]
        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        [MinLength(ValidationConstants.TeamNameMinLength)]
        [RegularExpression(ValidationConstants.TeamNameRegex)]
        public string Name { get; set; } = null!;

        [JsonProperty("Nationality")]
        [MaxLength(ValidationConstants.TeamNationalityMaxLength)]
        [MinLength(ValidationConstants.TeamNationalityMinLength)]
        public string? Nationality { get; set; }

        [JsonProperty("Trophies")]
        public string? Trophies { get; set; } 

        [JsonProperty("Footballers")]
        public int[] Footballers { get; set; } = null!;
    }
}
