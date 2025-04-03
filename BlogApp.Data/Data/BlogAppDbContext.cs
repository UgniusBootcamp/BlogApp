using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Data
{
    public class BlogAppDbContext : IdentityDbContext<User>
    {
        public BlogAppDbContext(DbContextOptions<BlogAppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
