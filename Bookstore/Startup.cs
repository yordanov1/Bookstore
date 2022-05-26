namespace Bookstore
{
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Bookstore.Infrastructure.Extensions;
    using Bookstore.Services.Books;
    using Bookstore.Services.Moderators;
    using Bookstore.Services.Statistics;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration) 
            => Configuration = configuration;

        public IConfiguration Configuration { get; }
  
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<BookstoreDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            // I want to have the default functionality for Users , default User identity User 
            // and I want you to keep them in the database in BookstoreDbContext
            services
                .AddDefaultIdentity<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BookstoreDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();

            services.AddTransient<IStatisticServices, StatisticServices>();
            services.AddTransient<IModeratorService, ModeratorService>();
            services.AddTransient<IBookService, BookService>();
        }

     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");                
                app.UseHsts();
            }

            app
               .UseHttpsRedirection()
               .UseStaticFiles()
               .UseRouting()
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {

                   endpoints.MapControllerRoute(
                       name: "Book Information",
                       pattern: "/Books/Information/{id}/{information}",
                       defaults: new { controller = "Books", action = "Information" });

                   //For Areas.If they exist do it this way.Otherwise in the way below.{area:exists}
                   endpoints.MapControllerRoute(
                       name: "Areas",
                       pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

                   endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");
                   endpoints.MapRazorPages();
               });
        }
    }
}
