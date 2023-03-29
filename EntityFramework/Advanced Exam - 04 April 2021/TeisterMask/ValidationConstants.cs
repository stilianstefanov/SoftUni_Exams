namespace TeisterMask
{
    public static class ValidationConstants
    {
        //Employee
        public const int EmployeeUserNameMaxLength = 40;

        public const int EmployeeUserNameMinLength = 3;

        public const string EmployeeUserNameRegex = @"^[a-zA-Z0-9]+$";

        public const string EmployeePhoneRegex = @"^[0-9]{3}-[0-9]{3}-[0-9]{4}$";

        //Project
        public const int ProjectNameMaxLength = 40;

        public const int ProjectNameMinLength = 2;

        //Task
        public const int TaskNameMaxLength = 40;

        public const int TaskNameMinLength = 2;
    }
}
