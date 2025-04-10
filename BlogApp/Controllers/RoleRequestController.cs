using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Helpers.Roles;
using BlogApp.Models.RoleRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class RoleRequestController(
        IRoleRequestService roleRequestService
        ) : Controller
    {
        [HttpGet(ControllerConstants.UserRoleRequest)]
        [Authorize]
        public async Task<IActionResult> UserRoleRequest()
        {
            if(User.IsInRole(UserRoles.Admin))
                return RedirectToAction(ControllerConstants.AllRoleRequest, ControllerConstants.RoleRequest);

            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            
            var roleRequests = await roleRequestService.GetUserRoleRequestsAsync(userId);

            return View(roleRequests);
        }

        [HttpGet(ControllerConstants.CreateRoleRequest)]
        [Authorize]
        public async Task<IActionResult> CreateUserRoleRequest()
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            var roles = await roleRequestService.GetEligibleRolesAsync(userId);

            return PartialView(ControllerConstants._RoleRequestCreateModal, roles);
        }

        [HttpPost(ControllerConstants.CreateRoleRequest)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserRoleRequest(string roleId)
        {
            if(!ModelState.IsValid)
                return RedirectToAction(ControllerConstants.CreateUserRoleRequest);

            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await roleRequestService.CreateRoleRequestAsync(userId, roleId);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.RoleRequestCreated;

            return RedirectToAction(ControllerConstants.UserRoleRequest);
        }

        [HttpGet(ControllerConstants.AllRoleRequest)]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AllRoleRequest(string? roleId, string? query, int pageIndex = 1, int pageSize = 1)
        {
            var roleRequests = await roleRequestService.GetAllRoleRequestsAsync(roleId, query, pageIndex, pageSize);
            var allRoleRequestsViewModel = new AllRoleRequestsViewModel
            {
                roleId = roleId,
                query = query,
                pageSize = pageSize,
                roleRequests = roleRequests
            };

            return View(allRoleRequestsViewModel);
        }

        [HttpPost(ControllerConstants.ConfirmRoleRequest)]
        [Authorize(Roles = UserRoles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRoleRequest(int roleRequestId)
        {
            await roleRequestService.ApproveRoleRequestAsync(roleRequestId);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.RoleRequestApproved;

            return RedirectToAction(ControllerConstants.AllRoleRequest);
        }

        [HttpPost(ControllerConstants.DeleteRoleRequest)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleRequest(int roleRequestId)
        {
            await roleRequestService.DeleteRoleRequestAsync(roleRequestId);

            var isAdmin = User.IsInRole(UserRoles.Admin);

            TempData[ControllerConstants.SnackbarMessage] = ControllerConstants.RoleRequestDeleted;

            if (isAdmin)
            {
                return RedirectToAction(ControllerConstants.AllRoleRequest);
            }
            else
            {
                return RedirectToAction(ControllerConstants.UserRoleRequest);
            }
        }
    }
}
