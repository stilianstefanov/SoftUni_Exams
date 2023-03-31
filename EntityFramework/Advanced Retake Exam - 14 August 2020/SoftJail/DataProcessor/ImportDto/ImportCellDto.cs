namespace SoftJail.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportCellDto
    {
        [JsonProperty("CellNumber")]
        [Range(ValidationConstants.CellNumberMinValue, ValidationConstants.CellNumberMaxValue)]
        public int CellNumber { get; set; }

        [JsonProperty("HasWindow")]
        [Required]
        public bool HasWindow { get; set; }
    }
}
