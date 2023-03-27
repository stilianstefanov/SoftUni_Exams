namespace Trucks
{
    public static class ValidationConstants
    {
        //Truck
        public const int TruckRegistrationNumberLength = 8;

        public const int TruckVinNumberLength = 17;

        public const int TruckTankCapacityMinLength = 950;

        public const int TruckTankCapacityMaxLength = 1420;

        public const int TruckCargoCapacityMinLength = 5000;

        public const int TruckCargoCapacityMaxLength = 29000;

        public const string TruckRegNumberRegex = @"^[A-Z]{2}[0-9]{4}[A-Z]{2}$";

        //Client
        public const int ClientNameMaxLength = 40;

        public const int ClientNameMinLength = 3;

        public const int ClientNationalityMaxLength = 40;

        public const int ClientNationalityMinLength = 2;

        //Dispatcher
        public const int DispatcherNameMaxLength = 40;

        public const int DispatcherNameMinLength = 2;
    }
}
