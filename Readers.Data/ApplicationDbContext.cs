using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Readers.Data.DataModels;

namespace Readers.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<UserShelf> UserShelves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.BookId })
                .IsUnique();
            builder.Entity<UserShelf>()
                .HasIndex(us => new { us.UserId, us.BookId })
                .IsUnique();
        }
    }

}

