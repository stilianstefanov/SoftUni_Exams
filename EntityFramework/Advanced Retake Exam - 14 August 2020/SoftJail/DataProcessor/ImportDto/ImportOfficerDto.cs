namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        [XmlElement("Name")]
        [MaxLength(ValidationConstants.OfficeFullNameMaxLength)]
        [MinLength(ValidationConstants.OfficerFullNameMinLength)]
        [Required]
        public string? FullName { get; set; }

        [XmlElement("Money")]
        [Required]
        public decimal? Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        public string? Position { get; set; }

        [XmlElement("Weapon")]
        [Required]
        public string? Weapon { get; set; }

        [XmlElement("DepartmentId")]
        [Required]
        public int? DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportOfficerPrisonerDto[] Prisoners { get; set; } = null!;
    }
}
