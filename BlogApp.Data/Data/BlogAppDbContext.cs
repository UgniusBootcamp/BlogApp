using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Data
{
    public class BlogAppDbContext : IdentityDbContext<User>
    {
        public BlogAppDbContext(DbContextOptions<BlogAppDbContext> options) : base(options) { }

        public DbSet<RoleRequest> RoleRequests { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleRequest>(entity =>
            {
                entity.HasIndex(r => new { r.UserId, r.RoleId }).IsUnique();

                entity.HasOne(r => r.User).WithMany(u => u.RoleRequests).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Role).WithMany().HasForeignKey(r => r.RoleId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Article>(entity =>
            {
                entity.HasOne(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(builder);
        }


    }
}
