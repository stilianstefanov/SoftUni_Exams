namespace Footballers
{
    public static class ValidationConstants
    {
        //Foorballer
        public const int FootBallerNameMaxLength = 40;

        public const int FootBallerNameMinLength = 2;

        //Team
        public const int TeamNameMaxLength = 40;

        public const int TeamNameMinLength = 3;

        public const int TeamNationalityMaxLength = 40;

        public const int TeamNationalityMinLength = 2;

        public const string TeamNameRegex = @"^[a-zA-Z0-9\s.-]+$";

        //Coach
        public const int CoachNameMaxLength = 40;

        public const int CoachNameMinLength = 2;
    }
}
