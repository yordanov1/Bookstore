namespace Bookstore.Infrastructure
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var data = scopedServices.ServiceProvider.GetService<BookstoreDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            return app;
        }

        private static void SeedCategories(BookstoreDbContext data)
        {            


            if (data.Genres.Any())
            {
                return;
            }

            data.Genres.AddRange(new[]
            {
                new Genre{ Name = "Fantasy"},
                new Genre{ Name = "Adventure"},
                new Genre{ Name = "Romance"},
                new Genre{ Name = "Mystery"},
                new Genre{ Name = "Horror"},
                new Genre{ Name = "Thriller"},
                new Genre{ Name = "Paranormal"},
                new Genre{ Name = "Historical"},
                new Genre{ Name = "Science"},
                new Genre{ Name = "Children’s"},
                new Genre{ Name = "Memoir"},
                new Genre{ Name = "Cooking"},
                new Genre{ Name = "Art"},
                new Genre{ Name = "Development"},
                new Genre{ Name = "Motivational"},
                new Genre{ Name = "Health"},
                new Genre{ Name = "Travel"},
                new Genre{ Name = "Families & Relationships"},
                new Genre{ Name = "Humor"},             
            });

            data.SaveChanges();
        }
    }
}
