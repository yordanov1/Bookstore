namespace Bookstore.Data
{
    using Bookstore.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class BookstoreDbContext : IdentityDbContext
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Administrator> Administrators { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Book>()
                .HasOne(g => g.Genre)
                .WithMany(b => b.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Book>()
                .HasOne(b => b.Administrator)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AdministratorId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .Entity<Administrator>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Administrator>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
