namespace VaporStore.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportGameDto
    {
        [JsonProperty("Name")]
        public string? Name { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [JsonProperty("ReleaseDate")]
        public string? ReleaseDate { get; set; }

        [JsonProperty("Developer")]
        public string? Developer { get; set; }

        [JsonProperty("Genre")]
        public string? Genre { get; set; }

        [JsonProperty("Tags")]
        public string[]? Tags { get; set; }
    }
}
