namespace BlogApp.Data.Constants
{
    public static class ServiceConstants
    {
        public const string InvalidCredentials = "Login credentials are not valid";
        public const string EmailIsAlreadyInUse = "Email is already in use";
        public const string PasswordsDoNotMatch = "Passwords do not match";
        public const string EmailIsNotConfirmed = "Email is not confirmed";
        public const string UsersNotFound = "User is not found";
        public const string InvalidEmailConfrimationRequest = "Invalid Email Confirmation Request";
        public const string InvalidPasswordResetRequest = "Invalid Password Reset Request";
        public const string UserUpdateFailed = "User update failed";
        public const string SingInFailed = "Sign in failed";
        public const string SignInFailedMessage = "Sign in failed. Please check your credentials and try again.";
        public const string ArgumentsCannotBeNull = "Arguments cannot be null";
        public const string MessageBody = "<html>" +
            "<body style='font-family:Arial, sans-serif; color:#444;'>" +
            "{0}" +
            " <p style='font-size:16px;'>Thanks for using our blog app system!</p>" +
            "</body>" +
            "</html>";
        public const string Token ="token";
        public const string Email = "email";
        public const string EmailConfirmation = "Email Confirmation";
        public const string ConfirmEmail = "Confirm Email";
        public const string PasswordReset = "Password Reset";
        public const string ResetPassword = "Reset Password";
        public const string Button = "<a href='{0}' style='display:inline-block;padding:10px 20px;font-size:16px;color:#fff;background-color:#007bff;text-decoration:none;border-radius:5px;'>{1}</a>";
        public const string RoleRequestNotFound = "Role request not found";
        public const string RoleNotFound = "Role not found";
        public const string UserAlreadyInRole = "User already in role";
        public static readonly string[] AllowedTypes = { "image/jpeg", "image/png", "image/gif" };
        public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        public const string AvailableFileTypes = "Available file types: jpg, jpeg, png, gif";
        public const string ArticleNotFound = "Article not found";
        public const string ArticleNotBelongsToUser = "Article does not belong to user";
        public const string CommentNotFound = "Comment not found";
        public const string CommentNotBelongsToUser = "Comment does not belong to user";
    }
}
