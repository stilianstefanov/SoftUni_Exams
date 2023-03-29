namespace Theatre.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Actor")]
    public class ExportCastDto
    {
        [XmlAttribute("FullName")]
        public string FullName { get; set; } = null!;

        [XmlAttribute("MainCharacter")]
        public string MainCharacter { get; set; } = null!;
    }
}
