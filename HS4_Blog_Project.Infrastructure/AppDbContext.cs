using HS4_Blog_Project.Domain.Entities;
using HS4_Blog_Project.Infrastructure.EntityTypeConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        //new lendigi zaman base e yani normal yukarıya optionsu gönder diyoruz.

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Post> Posts { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        //sonrada burayı acıcaz
        //public DbSet<Like> Likes { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GenreConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new AppUserConfig());
            builder.ApplyConfiguration(new AuthorConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new LikeConfig());

            base.OnModelCreating(builder);
        }
    }
}
