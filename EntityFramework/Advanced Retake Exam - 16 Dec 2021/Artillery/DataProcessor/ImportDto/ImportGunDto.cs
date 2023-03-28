namespace Artillery.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportGunDto
    {
        [JsonProperty("ManufacturerId")]
        public int ManufacturerId { get; set; }

        [JsonProperty("GunWeight")]
        [Range(ValidationConstants.GunWeightMinValue, ValidationConstants.GunWeightMaxValue)]
        public int GunWeight { get; set; }

        [JsonProperty("BarrelLength")]
        [Range(ValidationConstants.GunBarrelLengthMinValue, ValidationConstants.GunBarrelLengthMaxValue)]
        public double BarrelLength { get; set; }

        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }

        [JsonProperty("Range")]
        [Range(ValidationConstants.GunRangeMinValue, ValidationConstants.GunRangeMaxValue)]
        public int Range { get; set; }

        [JsonProperty("GunType")]
        public string? GunType { get; set; }

        [JsonProperty("ShellId")]
        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public ImportGunCountryIdDto[] CountryIds { get; set; } = null!;
    }
}
