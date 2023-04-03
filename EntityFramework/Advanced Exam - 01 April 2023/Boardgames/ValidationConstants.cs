namespace Boardgames
{
    public static class ValidationConstants
    {
        //Boardgame
        public const int BoardGameMaxLength = 20;

        public const int BoardGameMinLength = 10;

        public const double BoardGameRatingMaxValue = 10.00;

        public const double BoardGameRatingMinValue = 1;

        public const int BoardGameYearPublishedMaxValue = 2023;

        public const int BoardGameyearPublishedMinValue = 2018;

        //Seller
        public const int SellerNameMaxLength = 20;

        public const int SellernameMinLength = 5;

        public const int SellerAddressMaxLength = 30;

        public const int SellerAddressMinlength = 2;

        public const string SellerWebsiteRegex = @"^www\.[a-zA-Z0-9-]+\.com$";

        //Creator
        public const int CreatorNameMaxLength = 7;

        public const int CreatorNameMinlength = 2;
    }
}
