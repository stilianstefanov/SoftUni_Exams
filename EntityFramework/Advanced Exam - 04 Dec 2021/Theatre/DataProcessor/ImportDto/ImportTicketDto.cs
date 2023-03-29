namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportTicketDto
    {
        [JsonProperty("Price")]
        [Range(typeof(decimal), ValidationConstants.TicketPriceMinValue, ValidationConstants.TicketPriceMaxValue)]
        public decimal Price { get; set; }

        [JsonProperty("RowNumber")]
        [Range(ValidationConstants.TicketRowNumberMinValue, ValidationConstants.TicketRowNumberMaxValue)]
        public int RowNumber { get; set; }

        [JsonProperty("PlayId")]
        public int PlayId { get; set; }
    }
}
