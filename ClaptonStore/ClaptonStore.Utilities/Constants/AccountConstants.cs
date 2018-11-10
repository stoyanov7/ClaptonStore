namespace ClaptonStore.Utilities.Constants
{
    public static class AccountConstants
    {
        public const string LoggedInUser = "User logged in.";

        public const string InvalidLoginAttempt = "Invalid login attempt.";

        public const string CreatedNewAccount = "User created a new account with password.";

        public const string UserLoggedOut = "User logged out.";

        public const string ResetPassword = "Reset Password";

        public const string ResetPasswordLink =
            "Please reset your password by clicking here: <a href='{0}'>link</a>";

        public const string SuppliedCode = "A code must be supplied for password reset.";

        public const string UnableToLoadUser = "Unable to load user with ID '{0}'.";
    }
}