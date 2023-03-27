namespace Trucks.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportClientDto
    {
        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        [MinLength(ValidationConstants.ClientNameMinLength)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
        [MinLength(ValidationConstants.ClientNationalityMinLength)]
        [JsonProperty("Nationality")]
        public string? Nationality { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; } = null!;

        [JsonProperty("Trucks")]
        public int[] TruckIds { get; set; }
    }
}
