namespace SoftJail.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Prisoner")]
    public class ExportPrisonerDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; } = null!;

        [XmlArray("EncryptedMessages")]
        public ExportMessageDto[] EncryptedMessages { get; set; } = null!;
    }
}
