namespace Trucks.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Despatcher")]
    public class ExportDispatcherDto
    {
        [XmlAttribute("TrucksCount")]
        public string TrucksCount { get; set; } = null!;

        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; } = null!;

        [XmlArray("Trucks")]
        public ExportTruckDto[] Trucks { get; set; } = null!;
    }
}
