using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Data.Interfaces
{
    public interface IRoleRequestRepository
    {
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
        /// Method to get role requests by user id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user roleRequests</returns>
        public Task<IEnumerable<RoleRequest>> GetRoleRequestsAsync(string userId);

        /// <summary>
        /// Method to get role requests by role id
        /// </summary>
        /// <param name="roleRequestId">role request id</param>
        /// <returns>role request</returns>
        public Task<RoleRequest?> GetRoleRequestAsync(int roleRequestId);

        /// <summary>
        /// Get paginated role requests
        /// </summary>
        /// <param name="roleId">role id</param>
        /// <param name="query">user name and surname</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated role requests</returns>
        public Task<PaginatedList<RoleRequest>> GetRoleRequestsAsync(string? roleId, string? query, int pageIndex, int pageSize);
    }
}
