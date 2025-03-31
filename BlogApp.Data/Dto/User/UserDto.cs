namespace BlogApp.Data.Dto.User
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; } = null!;
        public IList<string> Roles { get; set; } = [];
        public bool EmailConfirmed { get; set; }
    }
}
