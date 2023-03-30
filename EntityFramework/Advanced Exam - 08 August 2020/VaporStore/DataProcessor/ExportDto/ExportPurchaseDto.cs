namespace VaporStore.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class ExportPurchaseDto
    {
        [XmlElement("Card")]
        public string Card { get; set; } = null!;

        [XmlElement("Cvc")]
        public string Cvc { get; set; } = null!;

        [XmlElement("Date")]
        public string Date { get; set; } = null!;

        [XmlElement("Game")]
        public ExportGameDto Game { get; set; } = null!;
    }
}
