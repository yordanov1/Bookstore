namespace Bookstore.Infrastructure.Extensions
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using static Bookstore.Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var servicesScope = app.ApplicationServices.CreateScope();
            var services = servicesScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<BookstoreDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<BookstoreDbContext>();

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

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            //If we want to run a method that is asynchronous but we want to work synchronously
            //Run all and wait
            Task
                .Run(async ()=>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@crs.com";
                    const string adminPassword = "admin12";   

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
