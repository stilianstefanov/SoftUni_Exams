namespace Boardgames.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportSellerDto
    {

        [JsonProperty("Name")]
        [Required]
        [MaxLength(ValidationConstants.SellerNameMaxLength)]
        [MinLength(ValidationConstants.SellernameMinLength)]
        public string Name { get; set; } = null!;

        [JsonProperty("Address")]
        [Required]
        [MaxLength(ValidationConstants.SellerAddressMaxLength)]
        [MinLength(ValidationConstants.SellerAddressMinlength)]
        public string Address { get; set; } = null!;

        [JsonProperty("Country")]
        [Required]
        public string Country { get; set; } = null!;

        [JsonProperty("Website")]
        [Required]
        [RegularExpression(ValidationConstants.SellerWebsiteRegex)]
        public string Website { get; set; } = null!;

        [JsonProperty("Boardgames")]
        public int[] Boardgames { get; set; } = null!;
    }
}
