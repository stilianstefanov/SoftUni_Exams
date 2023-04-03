namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;


    [XmlType("Boardgame")]
    public class ImportBoardGameDto
    {

        [XmlElement("Name")]
        [Required]
        [MaxLength(ValidationConstants.BoardGameMaxLength)]
        [MinLength(ValidationConstants.BoardGameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Rating")]
        [Required]
        [Range(ValidationConstants.BoardGameRatingMinValue, ValidationConstants.BoardGameRatingMaxValue)]
        public double Rating { get; set; }

        [XmlElement("YearPublished")]
        [Required]
        [Range(ValidationConstants.BoardGameyearPublishedMinValue, ValidationConstants.BoardGameYearPublishedMaxValue)]
        public int YearPublished { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        [Range(0, 4)]
        public int CategoryType { get; set; }

        [XmlElement("Mechanics")]
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
