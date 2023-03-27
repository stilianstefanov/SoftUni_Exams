namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportDespatcherDto[] dtos = Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

            ICollection<Despatcher> despatchers = new List<Despatcher>();
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                if (string.IsNullOrEmpty(dto.Position))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = dto.Name,
                    Position = dto.Position,
                };

                foreach (var truckDto in dto.Trucks)
                {
                    if (string.IsNullOrEmpty(truckDto.VinNumber))
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }
                  

                    despatcher.Trucks.Add(new Truck()
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType
                    });
                }

                despatchers.Add(despatcher);
                sb.AppendLine($"Successfully imported despatcher - {despatcher.Name} with {despatcher.Trucks.Count} trucks.");
            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var sb = new StringBuilder();

            ImportClientDto[] dtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString)!;

            ICollection<Client> clients = new List<Client>();
            foreach (var dto in dtos)
            {
                if (string.IsNullOrEmpty(dto.Nationality))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                if (dto.Type == "usual")
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                Client client = new Client()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality,
                    Type = dto.Type
                };

                foreach (var truckId in dto.TruckIds.Distinct())
                {
                    if (!context.Trucks.Any(t => t.Id == truckId))
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    client.ClientsTrucks.Add(new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId
                    });
                }

                clients.Add(client);
                sb.AppendLine($"Successfully imported client - {client.Name} with {client.ClientsTrucks.Count} trucks.");
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T deserializedDtos =
                (T)xmlSerializer.Deserialize(reader);

            return deserializedDtos;
        }

    }
}