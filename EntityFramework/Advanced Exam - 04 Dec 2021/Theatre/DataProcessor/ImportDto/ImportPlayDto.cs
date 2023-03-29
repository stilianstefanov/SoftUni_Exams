namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
   

    [XmlType("Play")]
    public class ImportPlayDto
    {
        [XmlElement("Title")]
        [MinLength(ValidationConstants.PlayeTitleMinValue)]
        [MaxLength(ValidationConstants.PlayeTitleMaxValue)]
        public string? Title { get; set; }

        [XmlElement("Duration")]
        public string? Duration { get; set; }

        [XmlElement("Raiting")]
        [Range(ValidationConstants.PlayRatingMinValue, ValidationConstants.PlayeRatingMaxValue)]
        public double Rating { get; set; }

        [XmlElement("Genre")]
        public string? Genre { get; set; }

        [XmlElement("Description")]
        [MaxLength(ValidationConstants.PlayeDescriptionMaxLength)]
        public string? Description { get; set; }

        [XmlElement("Screenwriter")]
        [MinLength(ValidationConstants.PlayScreenWriterMinLength)]
        [MaxLength(ValidationConstants.PlayScreenWriterMaxLength)]
        public string? Screenwriter { get; set; }
    }
}
