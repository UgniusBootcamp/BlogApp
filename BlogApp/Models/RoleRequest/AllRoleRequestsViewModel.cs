using BlogApp.Data.Dto.RoleRequest;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Models.RoleRequest
{
    public class AllRoleRequestsViewModel
    {
        public string? roleId { get; set; }
        public string? query { get; set; }
        public int pageSize { get; set; }
        public PaginatedList<RoleRequestDetailDto> roleRequests { get; set; } = null!;
    }
}
