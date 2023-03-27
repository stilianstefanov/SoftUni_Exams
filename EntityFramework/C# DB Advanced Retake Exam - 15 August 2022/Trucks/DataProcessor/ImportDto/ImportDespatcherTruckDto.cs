namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;

    [XmlType("Truck")]
    public class ImportDespatcherTruckDto
    {
        [MaxLength(ValidationConstants.TruckRegistrationNumberLength)]
        [MinLength(ValidationConstants.TruckRegistrationNumberLength)]
        [RegularExpression(ValidationConstants.TruckRegNumberRegex)]
        public string RegistrationNumber { get; set; } = null!;

        [MaxLength(ValidationConstants.TruckVinNumberLength)]
        [MinLength(ValidationConstants.TruckVinNumberLength)]
        public string? VinNumber { get; set; } 

        [Range(ValidationConstants.TruckTankCapacityMinLength, ValidationConstants.TruckTankCapacityMaxLength)]
        public int TankCapacity { get; set; }

        [Range(ValidationConstants.TruckCargoCapacityMinLength, ValidationConstants.TruckCargoCapacityMaxLength)]
        public int CargoCapacity { get; set; }

        public int CategoryType { get; set; }

        public int MakeType { get; set; }
    }
}
