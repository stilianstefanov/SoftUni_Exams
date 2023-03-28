namespace Artillery.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class ExportCountryDto
    {
        [XmlAttribute("Country")]
        public string CountryName { get; set; } = null!;

        [XmlAttribute("ArmySize")]
        public string ArmySize { get; set; } = null!;
    }
}
