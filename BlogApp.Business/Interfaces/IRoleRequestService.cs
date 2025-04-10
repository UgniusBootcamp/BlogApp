using BlogApp.Data.Dto.RoleRequest;
using BlogApp.Data.Dto.Roles;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Business.Interfaces
{
    public interface IRoleRequestService
    {
        public Task<IEnumerable<RoleRequestListDto>> GetUserRoleRequestsAsync(string userId);
        public Task<PaginatedList<RoleRequestDetailDto>> GetAllRoleRequestsAsync(string? roleId, string? query, int pageIndex, int pageSize);
        public Task CreateRoleRequestAsync(string userId, string roleId);
        public Task DeleteRoleRequestAsync(int roleRequestId);
        public Task ApproveRoleRequestAsync(int roleRequestId);
        public Task<IEnumerable<RoleDto>> GetEligibleRolesAsync(string userId);
    }
}
