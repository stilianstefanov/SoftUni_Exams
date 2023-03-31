namespace SoftJail.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportMailDto
    {
        [JsonProperty("Description")]
        [Required]
        public string? Description { get; set; }

        [JsonProperty("Sender")]
        [Required]
        public string? Sender { get; set; }
       
        [JsonProperty("Address")]
        [RegularExpression(ValidationConstants.MailAddressRegex)]
        [Required]
        public string? Address { get; set; } 
    }
}
