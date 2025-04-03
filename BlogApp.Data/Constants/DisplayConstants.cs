using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlogApp.Data.Constants
{
    public static class DisplayConstants
    {
        public const string usernameOrEmailIsRequired = "Username or Email is required";
        public const string NameCannotExceed255Characters = "Name cannot exceed 255 characters";
        public const string usernameOrEmail = "Username or Email";
        public const string passwordIsRequired = "Password is required";
        public const string passwordMustBeBetween8And100Characters = "Password must be between 8 and 100 characters";
        public const string passwordMustContain = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character";
        public const string password = "Password";
        public const string pleaseConfirmYourPassword = "Please confirm your password";
        public const string confirmPassword = "Confirm Password";
        public const string passwordsDoNotMatch = "Passwords do not match";
        public const string emailAddress = "Email Address";
        public const string emailAddressIsRequired = "Email address is required";
        public const string pleaseEnterAValidEmailAddress = "Please enter a valid email address";
        public const string email = "Email";
        public const string confirmEmail = "Confirm Email";
        public const string emailAddressesDoNotMatch = "Email addresses do not match";
        public const string pleaseConfirmYourEmailAddress = "Please confirm your email address";
        public const string pleaseEnterYourFirstName = "Please enter your first name";
        public const string firstName = "First Name";
        public const string firstNameCannotExceed50Characters = "First name cannot exceed 50 characters";
        public const string pleaseEnterYourLastName = "Please enter your last name";
        public const string lastName = "Last Name";
        public const string lastNameCannotExceed50Characters = "Last name cannot exceed 50 characters";
    }
}
