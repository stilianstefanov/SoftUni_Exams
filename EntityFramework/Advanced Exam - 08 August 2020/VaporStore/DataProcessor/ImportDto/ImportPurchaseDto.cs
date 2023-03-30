namespace VaporStore.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
  

    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlAttribute("title")]
        [Required]
        public string? GameTitle { get; set; }

        [XmlElement("Type")]
        [Required]
        public string? Type { get; set; }

        [XmlElement("Key")]
        [RegularExpression(ValidationConstants.PurchaseProductKeyRegex)]
        [Required]
        public string? ProductKey { get; set; }

        [XmlElement("Date")]
        [Required]
        public string? Date { get; set; }

        [XmlElement("Card")]
        [RegularExpression(ValidationConstants.CardNumberRegex)]
        [Required]
        public string? CardNumber { get; set; }
    }
}
