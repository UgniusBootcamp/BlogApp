using AutoMapper;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.RoleRequest;
using BlogApp.Data.Dto.Roles;
using BlogApp.Data.Helpers.Exceptions;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Helpers.Roles;
using BlogApp.Data.Interfaces;

namespace BlogApp.Business.Services
{
    public class RoleRequestService(
        IRoleRequestRepository roleRequestRepository,
        IAccountRepository accountRepository,
        IMapper mapper
        ) : IRoleRequestService
    {
        /// <summary>
        /// Method to approve a role request.
        /// </summary>
        /// <param name="roleRequestId">role request id</param>
        /// <exception cref="NotFoundException">if role request not found</exception>
        public async Task ApproveRoleRequestAsync(int roleRequestId)
        {
            var roleRequest = await roleRequestRepository.GetRoleRequestAsync(roleRequestId);

            if (roleRequest == null)
                throw new NotFoundException(ServiceConstants.RoleRequestNotFound);

            await accountRepository.AddUserToRole(roleRequest.UserId, roleRequest.RoleId);
            await roleRequestRepository.DeleteRoleRequestAsync(roleRequestId);
        }

        /// <summary>
        /// Method to create role request
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roleId">role id</param>
        /// <exception cref="NotFoundException">if user or role not found</exception>
        /// <exception cref="BusinessRuleValidationException">if user already in role</exception>
        public async Task CreateRoleRequestAsync(string userId, string roleId)
        {
            var user = await accountRepository.FindUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var role = await accountRepository.GetRoleByIdAsync(roleId);
            if (role == null)
                throw new NotFoundException(ServiceConstants.RoleNotFound);

            if (await accountRepository.IsUserInRoleAsync(user, [role.Name]))
                throw new BusinessRuleValidationException(ServiceConstants.UserAlreadyInRole);

            await roleRequestRepository.CreateRoleRequestAsync(userId, roleId);
        }

        /// <summary>
        /// Method to delete role request
        /// </summary>
        /// <param name="roleRequestId">role request id</param>
        public async Task DeleteRoleRequestAsync(int roleRequestId)
        {
            await roleRequestRepository.DeleteRoleRequestAsync(roleRequestId);
        }

        /// <summary>
        /// Method to get paginated role request
        /// </summary>
        /// <param name="roleId">role id</param>
        /// <param name="query">user name and surname</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        public async Task<PaginatedList<RoleRequestDetailDto>> GetAllRoleRequestsAsync(string? query, int pageIndex, int pageSize)
        {
            var roleRequests = await roleRequestRepository.GetRoleRequestsAsync(query, pageIndex, pageSize);

            var mapped = new PaginatedList<RoleRequestDetailDto>(mapper.Map<IEnumerable<RoleRequestDetailDto>>(roleRequests.Items), roleRequests.PageIndex, roleRequests.TotalPages);

            return mapped;
        }

        /// <summary>
        /// Method to get eligible roles for user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>eligable roles to add</returns>
        /// <exception cref="NotFoundException">if user not found</exception>
        public async Task<IEnumerable<RoleDto>> GetEligibleRolesAsync(string userId)
        {
            var user = await accountRepository.FindUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var eligibleRoles = UserRoles.RequestRoles;

            var roles = await accountRepository.GetAllRolesAsync();
            var userRoles = await accountRepository.GetUserRolesAsync(user);
            var userRoleRequests = await roleRequestRepository.GetRoleRequestsAsync(userId);

            var rolesToAdd = roles
                .Where(r => eligibleRoles.Contains(r.Name))
                .Where(r => !userRoles.Any(ur => ur == r.Name))
                .Where(r => !userRoleRequests.Any(urr => urr.RoleId == r.Id));

            return mapper.Map<IEnumerable<RoleDto>>(rolesToAdd);
        }

        /// <summary>
        /// Method to get all user role requests
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user role requests</returns>
        /// <exception cref="NotFoundException">if user is not found</exception>
        public async Task<IEnumerable<RoleRequestListDto>> GetUserRoleRequestsAsync(string userId)
        {
            var user = await accountRepository.FindUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(ServiceConstants.UsersNotFound);

            var roleRequests = await roleRequestRepository.GetRoleRequestsAsync(userId);

            return mapper.Map<IEnumerable<RoleRequestListDto>>(roleRequests);
        }
    }
}
