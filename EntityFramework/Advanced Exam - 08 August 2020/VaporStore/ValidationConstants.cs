namespace VaporStore
{
    public static class ValidationConstants
    {      
        //User
        public const int UserUserNameMaxLength = 20;

        public const int UserUserNameMinLength = 3;

        public const string UserFullNameRegex = @"^[A-Z][a-z]+\s[A-Z][a-z]+$";

        public const int UserAgeMaxValue = 103;

        public const int UserAgeMinValue = 3;

        //Card
        public const string CardNumberRegex = @"^\d{4}\s\d{4}\s\d{4}\s\d{4}$";

        public const string CardCvcRegex = @"^\d{3}$";

        //Purchase
        public const string PurchaseProductKeyRegex = @"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$";
    }
}
