using Chess.Data;
using Chess.Models;
using Chess.Services.Interfaces;
using Chess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chess
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ChessDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                ConfigureIdentity(builder.Configuration, options);
            })
                .AddEntityFrameworkStores<ChessDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IChessEngineService, ChessEngineService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(ConfigurationManager configuration, IdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedEmail = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedEmail");
            options.SignIn.RequireConfirmedPhoneNumber = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedPhoneNumber");

            options.Password.RequireDigit = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireDigit");
            options.Password.RequireLowercase = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireLowercase");
            options.Password.RequireNonAlphanumeric = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireNonAlphanumeric");
            options.Password.RequireUppercase = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireUppercase");

            options.Password.RequiredLength = configuration
                .GetValue<int>("IdentityOptions:Password:RequiredLength");
        }
    }
}
