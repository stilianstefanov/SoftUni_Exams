namespace SoftJail
{
    public static class ValidationConstants
    {
        //Prisoner
        public const int PrisonerFullNameMaxLength = 20;

        public const int PrisonerFullNameMinLength = 3;

        public const string PrisonerNickNameRegex = @"^The [A-Z][a-z]+$";

        public const int PrisonerAgeMaxValue = 65;

        public const int PrisonerAgeMinValue = 18;

        //Officer
        public const int OfficeFullNameMaxLength = 30;

        public const int OfficerFullNameMinLength = 3;

        //Cell
        public const int CellNumberMaxValue = 1000;

        public const int CellNumberMinValue = 1;

        //Mail
        public const string MailAddressRegex = @"^[a-zA-Z0-9\s]+str\.$";

        //Department
        public const int DepartmentNameMaxLength = 25;

        public const int DepartmentNameMinLength = 3;
    }
}
