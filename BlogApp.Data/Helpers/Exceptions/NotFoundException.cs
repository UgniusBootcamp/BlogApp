namespace BlogApp.Data.Helpers.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
    }
}
