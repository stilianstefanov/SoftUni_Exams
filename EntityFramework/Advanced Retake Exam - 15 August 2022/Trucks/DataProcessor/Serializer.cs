namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;
    using ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            ExportDispatcherDto[] despatchers = context.Despatchers
                .Where(d => d.Trucks.Any())
                .ToArray()
                .Select(d => new ExportDispatcherDto()
                {
                    TrucksCount = d.Trucks.Count.ToString(),
                    DespatcherName = d.Name,
                    Trucks = d.Trucks.Select(t => new ExportTruckDto()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            return Serialize<ExportDispatcherDto[]>(despatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clientsWithTrucks = context.Clients
                .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))              
                .ToArray()
                .Select(c => new
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks.Where(ct => ct.Truck.TankCapacity >= capacity)
                                            .Select(ct => new
                                            {
                                                TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                                                VinNumber = ct.Truck.VinNumber,
                                                TankCapacity = ct.Truck.TankCapacity,
                                                CargoCapacity = ct.Truck.CargoCapacity,
                                                CategoryType = ct.Truck.CategoryType.ToString(),
                                                MakeType = ct.Truck.MakeType.ToString()
                                            })
                                            .OrderBy(t => t.MakeType)
                                            .ThenByDescending(t => t.CargoCapacity)
                                            .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clientsWithTrucks, Formatting.Indented);
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
