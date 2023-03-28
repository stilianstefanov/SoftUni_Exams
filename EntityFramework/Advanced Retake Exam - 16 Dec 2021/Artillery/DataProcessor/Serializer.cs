namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shellsWithGuns = context.Shells
                .Where(s => s.ShellWeight >= shellWeight)
                .ToArray()
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                            .Where(g => g.GunType == GunType.AntiAircraftGun)
                            .Select(g => new
                            {
                                GunType = g.GunType.ToString(),
                                GunWeight = g.GunWeight,
                                BarrelLength = g.BarrelLength,
                                Range = g.Range > 3000 ? "Long-range" : "Regular range"
                            })
                            .OrderByDescending(g => g.GunWeight)
                            .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(shellsWithGuns, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            ExportGunDto[] exportGuns = context.Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength)
                .ToArray()
                .Select(g => new ExportGunDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight.ToString(),
                    BarrelLength = g.BarrelLength.ToString(),
                    Range = g.Range.ToString(),
                    Countries = g.CountriesGuns
                                 .Where(cg => cg.Country.ArmySize > 4500000)
                                 .OrderBy(cg => cg.Country.ArmySize)
                                 .Select(cg => new ExportCountryDto()
                                 {
                                     CountryName = cg.Country.CountryName,
                                     ArmySize = cg.Country.ArmySize.ToString()
                                 })
                                 .ToArray()
                })
                .ToArray();

            return Serialize<ExportGunDto[]>(exportGuns, "Guns");
        }

        private static string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot =
                new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
