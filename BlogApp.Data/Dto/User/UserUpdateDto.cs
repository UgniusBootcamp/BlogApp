namespace BlogApp.Data.Dto.User
{
    public class UserUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
