using Application.Common;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Security.Claims;
using WebAPI.Extensions;

namespace WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.RegisterServices(typeof(Program));

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

            var app = builder.Build();

            app.RegisterPipelineComponents(typeof(Program));
            app.UseSerilogRequestLogging();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { RoleTypes.CONSUMER, RoleTypes.RESTAURANT_OWNER };
                foreach (var role in roles)
                {
                    if(!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            app.Run();
        }
    }

}