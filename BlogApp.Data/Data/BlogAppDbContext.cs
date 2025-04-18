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
        public DbSet<ArticleVote> ArticleVotes { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

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

            builder.Entity<ArticleVote>(entity =>
            {
                entity.HasIndex(av => new { av.ArticleId, av.UserId }).IsUnique();

                entity.HasOne(av => av.Article).WithMany(a => a.ArticleVotes).HasForeignKey(av => av.ArticleId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(av => av.User).WithMany(u => u.ArticleVotes).HasForeignKey(av => av.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.Article).WithMany(a => a.Comments).HasForeignKey(c => c.ArticleId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(builder);
        }


    }
}
