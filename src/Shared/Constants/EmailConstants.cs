namespace Shared.Constants
{
    public class EmailConstants
    {
        public const string ConfirmationEmailSubject = "Email confirmation link.";
        public const string ResetPasswordSubject = "Reset password.";
        public const string AddedToProject = "You have been added to project: ";
        public const string AddedToProjectSubject =  "Added to project.";
        public const string RemoveFromProject = "You have been removed from project: ";
        public const string RemoveFromProjectSubject = "Removed from project.";
        public const string FromMailingName = "AgileMS";
        public const string ConfirmEmail = "Please confirm your account by <a href='{0}'>clicking here</a>.";
        public const string ExternalLogin = "Please confirm your external login by <a href='{0}'>clicking here</a>.";
        public const string ConfirmResetPassword = "Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
    }
}
