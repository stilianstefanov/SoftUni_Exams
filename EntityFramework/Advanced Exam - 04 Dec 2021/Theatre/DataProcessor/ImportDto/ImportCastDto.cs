namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement("FullName")]
        [MinLength(ValidationConstants.CastFullNameMinLength)]
        [MaxLength(ValidationConstants.CastFullNameMaxLength)]
        public string? FullName { get; set; }

        [XmlElement("IsMainCharacter")]
        public bool IsMainCharacter { get; set; }

        [XmlElement("PhoneNumber")]
        [RegularExpression(ValidationConstants.CastPhoneNumberRegex)]
        public string? PhoneNumber { get; set; }

        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
