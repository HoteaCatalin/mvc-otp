namespace OTP.Helper
{
    public static class ErrorCodes
    {
        public static string InvalidUserId = "Invalid userId!";
        public static string InvalidCredentials = "Invalid userId or password!";

        public static string PasswordNotUsed = "Password exists that was not used";
        public static string PasswordWarning = "Password remains active only 30 seconds from now : ";
        public static string PasswordCreationFailed = "Sorry, an error was found while creating your password. Please try again.";
        public static string PasswordNotValid = "Password not valid anymore";
    }
}