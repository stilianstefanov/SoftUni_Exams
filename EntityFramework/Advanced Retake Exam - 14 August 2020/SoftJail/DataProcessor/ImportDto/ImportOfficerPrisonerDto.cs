namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Prisoner")]
    public class ImportOfficerPrisonerDto
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = null!;
    }
}
