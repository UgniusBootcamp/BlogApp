using BlogApp.Data.Dto.User;

namespace BlogApp.Data.Dto.RoleRequest
{
    public class RoleRequestDetailDto
    {
        public int Id { get; set; }
        public UserDetailDto User { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
}
