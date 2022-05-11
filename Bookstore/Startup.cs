namespace Bookstore
{
    using Bookstore.Data;
    using Bookstore.Infrastructure;
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
            // Искам да имам дефолтната функционалност за Юзъри , дефолтния Юзър идентити юзър 
            // и искам да ги пазиш в базата данни в BookstoreDbContext
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
               //За Юзърите
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");
                   endpoints.MapRazorPages();
               });
        }
    }
}
