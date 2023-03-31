namespace SoftJail.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Message")]
    public class ExportMessageDto
    {
        [XmlElement("Description")]
        public string Description { get; set; } = null!;
    }
}
