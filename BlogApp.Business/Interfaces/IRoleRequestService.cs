using BlogApp.Data.Dto.RoleRequest;
using BlogApp.Data.Dto.Roles;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Business.Interfaces
{
    public interface IRoleRequestService
    {
        /// <summary>
        /// Method to get user role requests
        /// </summary>
        /// <param name="userId"> user id</param>
        /// <returns>user role requests</returns>
        public Task<IEnumerable<RoleRequestListDto>> GetUserRoleRequestsAsync(string userId);

        /// <summary>
        /// Method to get paginated list of role requests
        /// </summary>
        /// <param name="query">name and surname search</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated list of roles</returns>
        public Task<PaginatedList<RoleRequestDetailDto>> GetAllRoleRequestsAsync(string? query, int pageIndex, int pageSize);

        /// <summary>
        /// Method to create role request
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roleId">role id</param>
        public Task CreateRoleRequestAsync(string userId, string roleId);

        /// <summary>
        /// Method to delete role request
        /// </summary>
        /// <param name="roleRequestId">role request id</param>
        public Task DeleteRoleRequestAsync(int roleRequestId);

        /// <summary>
        /// Method to approve role request
        /// </summary>
        /// <param name="roleRequestId">role request id</param>
        public Task ApproveRoleRequestAsync(int roleRequestId);

        /// <summary>
        /// Method to get eligible user roles to request
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>eligible user roles to request</returns>
        public Task<IEnumerable<RoleDto>> GetEligibleRolesAsync(string userId);
    }
}
