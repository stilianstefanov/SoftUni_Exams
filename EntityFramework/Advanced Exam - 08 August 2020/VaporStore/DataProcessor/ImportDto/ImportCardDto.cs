

namespace VaporStore.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportCardDto
    {
        [JsonProperty("Number")]
        [RegularExpression(ValidationConstants.CardNumberRegex)]
        [Required]
        public string? Number { get; set; }

        [JsonProperty("CVC")]
        [RegularExpression(ValidationConstants.CardCvcRegex)]
        [Required]
        public string? Cvc { get; set; }

        [JsonProperty("Type")]
        [Required]
        public string? Type { get; set; }
    }
}
