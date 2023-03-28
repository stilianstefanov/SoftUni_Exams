namespace Artillery
{
    public static class ValidationConstants
    {
        //Country
        public const int CountryNameMaxLength = 60;

        public const int CountryNameMinLength = 4;

        public const int CountryArmySizeMaxValue = 10_000_000;

        public const int CountryArmySizeMinValue = 50_000;

        //Manufacturer
        public const int ManNameMaxLength = 40;

        public const int ManNameMinLength = 4;

        public const int ManFoundedMaxLength = 100;

        public const int ManFoundedMinLength = 10;

        //Shell
        public const double ShellWeightMaxValue = 1_680;

        public const double ShellWeightMinValue = 2;

        public const int ShellCaliberMaxLength = 30;

        public const int ShellCaliberMinLength = 4;

        //Gun
        public const int GunWeightMaxValue = 1_350_000;

        public const int GunWeightMinValue = 100;

        public const double GunBarrelLengthMaxValue = 35.00;

        public const double GunBarrelLengthMinValue = 2.00;

        public const int GunRangeMaxValue = 100_000;

        public const int GunRangeMinValue = 1;
    }
}
