namespace Boardgames.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string CreatorName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ExportBoardgameDto[] Boardgames { get; set; } = null!;
    }
}
