namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;


    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement("Name")]
        [MaxLength(ValidationConstants.CoachNameMaxLength)]
        [MinLength(ValidationConstants.CoachNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Nationality")]
        public string? Nationality { get; set; }

        public ImportFootballerDto[] Footballers { get; set; } = null!;

    }
}
