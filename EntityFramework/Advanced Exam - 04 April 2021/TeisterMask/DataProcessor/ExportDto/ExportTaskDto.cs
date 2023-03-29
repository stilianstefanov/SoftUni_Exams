namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Task")]
    public class ExportTaskDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Label")]
        public string Label { get; set; } = null!;
    }
}
