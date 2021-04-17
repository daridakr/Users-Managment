using System;
using AuthSystem.Areas.Identity.Data;
using AuthSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(AuthSystem.Areas.Identity.IdentityHostingStartup))]
namespace AuthSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                    services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthDbContextConnection")));
                else
                    services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthDbContextConnection")));

                services.BuildServiceProvider().GetService<AuthDbContext>().Database.Migrate();

                services.AddDefaultIdentity<AppUser>(options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<AuthDbContext>();
            });
        }
    }
}