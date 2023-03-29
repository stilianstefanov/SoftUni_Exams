namespace Theatre
{
    public static class ValidationConstants
    {
        //Theatre
        public const int TheatreNameMaxLength = 30;

        public const int TheatreNameMinLength = 4;

        public const int TheatreNumberOfHallsMaxValue = 10;

        public const int TheatreNumberOfHallsMinValue = 1;

        public const int TheatreDirectorMinLength = 4;

        public const int TheatreDirectorMaxLength = 30;

        //Play
        public const int PlayeTitleMaxValue = 50;

        public const int PlayeTitleMinValue = 4;

        public const int PlayTimeSpanMinLength = 1;

        public const double PlayRatingMinValue = 0.00;

        public const double PlayeRatingMaxValue = 10.00;

        public const int PlayeDescriptionMaxLength = 700;

        public const int PlayScreenWriterMaxLength = 30;

        public const int PlayScreenWriterMinLength = 4;

        //Cast
        public const int CastFullNameMaxLength = 30;

        public const int CastFullNameMinLength = 4;

        public const string CastPhoneNumberRegex = @"^\+44-\d{2}-\d{3}-\d{4}$";

        //Ticket
        public const string TicketPriceMaxValue = "100.00";

        public const string TicketPriceMinValue = "1.00";

        public const int TicketRowNumberMaxValue = 10;

        public const int TicketRowNumberMinValue = 1;

    }
}
