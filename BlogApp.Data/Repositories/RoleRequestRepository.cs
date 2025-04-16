using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class RoleRequestRepository(BlogAppDbContext context) : IRoleRequestRepository
    {
        public async Task CreateRoleRequestAsync(string userId, string roleId)
        {
            await context.RoleRequests.AddAsync(new RoleRequest
            {
                UserId = userId,
                RoleId = roleId
            });

            await context.SaveChangesAsync();
        }

        public async Task DeleteRoleRequestAsync(int roleRequestId)
        {
            await context.RoleRequests
                .Where(r => r.Id == roleRequestId)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
        }

        public async Task<RoleRequest?> GetRoleRequestAsync(int roleRequestId)
        {
            return await context.RoleRequests
                 .Include(r => r.Role)
                 .Include(r => r.User)
                 .FirstOrDefaultAsync(r => r.Id == roleRequestId);
        }

        public async Task<IEnumerable<RoleRequest>> GetRoleRequestsAsync(string userId)
        {
            return await context.RoleRequests
               .Where(r => r.UserId == userId)
               .Include(r => r.Role)
               .Include(r => r.User)
               .ToListAsync();
        }

        public async Task<PaginatedList<RoleRequest>> GetRoleRequestsAsync(string? query, int pageIndex, int pageSize)
        {
            var roleRequests = context.RoleRequests
                .Include(r => r.Role)
                .Include(r => r.User)
                .AsQueryable();

            if (!String.IsNullOrEmpty(query))
            {
                var queryParts = query.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                roleRequests = queryParts.Length == 1
                    ? roleRequests.Where(rr =>
                        rr.User.Name!.ToLower().Contains(queryParts[0]) ||
                        rr.User.Surname!.ToLower().Contains(queryParts[0])
                    )
                    : roleRequests.Where(rr =>
                        ($"{rr.User.Name} {rr.User.Surname}").ToLower().Contains(query.ToLower())
                    );
            }

            var count = await roleRequests.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var items = await roleRequests
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<RoleRequest>(items, pageIndex, totalPages);
        }
    }
}
