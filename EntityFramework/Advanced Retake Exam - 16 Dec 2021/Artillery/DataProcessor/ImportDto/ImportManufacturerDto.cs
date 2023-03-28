namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        [XmlElement("ManufacturerName")]
        [MaxLength(ValidationConstants.ManNameMaxLength)]
        [MinLength(ValidationConstants.ManNameMinLength)]
        public string? ManufacturerName { get; set; }

        [MaxLength(ValidationConstants.ManFoundedMaxLength)]
        [MinLength(ValidationConstants.ManFoundedMinLength)]
        [XmlElement("Founded")]
        public string Founded { get; set; } = null!;
    }
}
