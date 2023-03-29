namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportTheatreDto
    {
        [JsonProperty("Name")]
        [MinLength(ValidationConstants.TheatreNameMinLength)]
        [MaxLength(ValidationConstants.TheatreNameMaxLength)]
        public string? Name { get; set; }

        
        [JsonProperty("NumberOfHalls")]
        [Range(ValidationConstants.TheatreNumberOfHallsMinValue, ValidationConstants.TheatreNumberOfHallsMaxValue)]
        public int NumberOfHalls { get; set; }

        [JsonProperty("Director")]
        [MinLength(ValidationConstants.TheatreDirectorMinLength)]
        [MaxLength(ValidationConstants.TheatreDirectorMaxLength)]
        public string? Director { get; set; }

        [JsonProperty("Tickets")]
        public ImportTicketDto[] Tickets { get; set; } = null!;
    }
}
