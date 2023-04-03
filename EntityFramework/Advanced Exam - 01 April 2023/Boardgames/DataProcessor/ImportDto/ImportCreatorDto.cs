namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [XmlElement("FirstName")]
        [Required]
        [MaxLength(ValidationConstants.CreatorNameMaxLength)]
        [MinLength(ValidationConstants.CreatorNameMinlength)]
        public string FirstName { get; set; } = null!;

        [XmlElement("LastName")]
        [Required]
        [MaxLength(ValidationConstants.CreatorNameMaxLength)]
        [MinLength(ValidationConstants.CreatorNameMinlength)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ImportBoardGameDto[] Boardgames { get; set; } = null!;
    }
}
