namespace Artillery.DataProcessor.ImportDto
{
    using Newtonsoft.Json;

    public class ImportGunCountryIdDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}
