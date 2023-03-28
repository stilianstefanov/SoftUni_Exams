namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportCountryDto[] countryDtos = Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            ICollection<Country> validCountries = new List<Country>();
            foreach (var dto in countryDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validCountries.Add(new Country()
                {
                    CountryName = dto.CountryName,
                    ArmySize = dto.ArmySize
                });

                sb.AppendLine(string.Format(SuccessfulImportCountry, dto.CountryName, dto.ArmySize));
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportManufacturerDto[] manDtos = Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            ICollection<Manufacturer> validManufacturers = new List<Manufacturer>();
            foreach (var dto in manDtos)
            {
                if (!IsValid(dto) || validManufacturers.Any(m => m.ManufacturerName == dto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = dto.ManufacturerName!,
                    Founded = dto.Founded,
                };

                string[] tokens = manufacturer.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                string countryName = tokens[tokens.Length - 1];
                string townName = tokens[tokens.Length - 2];

                validManufacturers.Add(manufacturer);
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, $"{townName}, {countryName}"));
            }

            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ImportShellDto[] shellDtos = Deserialize<ImportShellDto[]>(xmlString, "Shells");

            ICollection<Shell> validShells = new List<Shell>();
            foreach (var dto in shellDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validShells.Add(new Shell()
                {
                    ShellWeight = dto.ShellWeight,
                    Caliber = dto.Caliber!
                });

                sb.AppendLine(string.Format(SuccessfulImportShell, dto.Caliber, dto.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var sb = new StringBuilder();

            ImportGunDto[] gunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString)!;

            ICollection<Gun> validGuns = new List<Gun>();
            foreach (var gunDto in gunDtos)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isGunTypeValid = Enum.TryParse<GunType>(gunDto.GunType, out GunType gunType);
                if (!isGunTypeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = gunType,
                    ShellId = gunDto.ShellId
                };

                foreach (var countryIdDto in gunDto.CountryIds)
                {
                    gun.CountriesGuns.Add(new CountryGun()
                    {
                        Gun = gun,
                        CountryId = countryIdDto.Id
                    });
                }

                validGuns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType.ToString(), gun.GunWeight, gun.BarrelLength));
            }

            context.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
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