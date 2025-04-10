using Org.BouncyCastle.Asn1.Mozilla;

namespace BlogApp.Data.Constants
{
    public static class ControllerConstants
    {
        public const string Login = "Login";
        public const string Register = "Register";
        public const string Logout = "Logout";
        public const string EmailConfirmation = "EmailConfirmation";
        public const string PasswordReset = "PasswordReset";
        public const string PasswordResetConfirmEndpoint = "PasswordReset/Confirm";
        public const string Profile = "Profile";
        public const string ProfileUpdate = "Profile/Update";
        public const string Home = "Home";
        public const string Index = "Index";
        public const string UserRoleRequest = "UserRoleRequest";
        public const string RoleRequest = "RoleRequest";
        public const string ConfirmRoleRequest = $"ConfirmRoleRequest";
        public const string AllRoleRequest = "AllRoleRequest";
        public const string DeleteRoleRequest = "DeleteRoleRequest";
        public const string CreateRoleRequest = "CreateRoleRequest";
        public const string EligibleRoles = "EligibleRoles";
        public const string SnackbarMessage = "SnackbarMessage";
        public const string Password = "Password";
        public const string LogInSuccessful = "Log in Successful!";
        public const string LogOutSuccessful = "Log out Successful!";
        public const string Account = "Account";
        public const string Email = "Email";
        public const string ConfirmationEmailSent = "Confirmation Email has been sent. Chech Your Inbox.";
        public const string PasswordResetConfirm = "PasswordResetConfirm";
        public const string PasswordResetConfirmMessage = "Password Reset has been sent! Check Your Inbox";
        public const string PasswordHasBeenReset = "Password has been reset! You can now log in with your new password.";
        public const string ProfileHasBeenUpdated = "Profile has been updated!";
        public const string NotFound = "NotFound";
        public const string ServerError = "ServerError";
        public const string AccessDenied = "AccessDenied";
        public const string NotFoundEndpoint = "/Error/NotFound";
        public const string ServerErrorEndpoint = "/Error/ServerError";
        public const string AccessDeniedEndpoint = "/Error/AccessDenied";
        public const string LoginEndpoint = "/Account/Login";
        public const string AppAuth = "AppAuth";
        public const string RoleRequestCreated = "Role request has been created";
        public const string CreateUserRoleRequest = "CreateUserRoleRequest";
        public const string RoleRequestApproved = "Role request has been approved";
        public const string RoleRequestDeleted = "Role request has been deleted";
        public const string _RoleRequestCreateModal = "_RoleRequestCreateModal";
        public const string UpdateProfile = "UpdateProfile";
    }
}
