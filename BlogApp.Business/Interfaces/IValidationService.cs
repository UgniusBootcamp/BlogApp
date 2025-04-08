namespace BlogApp.Business.Interfaces
{
    public interface IValidationService
    {
        /// <summary>
        /// Method to check if password is valid
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="confirmPassword">confirmed password</param>
        void ValidateRegisterPassword(string password, string confirmPassword);
    }
}
