namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [XmlElement("Name")]
        [MaxLength(ValidationConstants.DispatcherNameMaxLength)]
        [MinLength(ValidationConstants.DispatcherNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string? Position { get; set; }

        [XmlArray("Trucks")]
        public ImportDespatcherTruckDto[] Trucks { get; set; }
    }
}
