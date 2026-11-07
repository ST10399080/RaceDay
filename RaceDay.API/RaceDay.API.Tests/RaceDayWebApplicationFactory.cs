using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using RaceDay.API.Data;
using System.Text;

namespace RaceDay.API.Tests;

public class RaceDayWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "RaceDay_Test_Key_12345678901234567890123456789012",
                ["Jwt:Issuer"] = "RaceDay.Tests",
                ["Jwt:Audience"] = "RaceDay.Tests"
            });
        });

        builder.ConfigureServices((context, services) =>
        {
            services.RemoveAll<
                DbContextOptions<RaceDayDbContext>>();

            services.RemoveAll<RaceDayDbContext>();

            services.AddDbContext<RaceDayDbContext>(
                options => options.UseInMemoryDatabase(
                    "RaceDayIntegrationTests"));

            services.PostConfigure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.Events ??= new JwtBearerEvents();

        options.Events.OnAuthenticationFailed = context =>
        {
            context.Response.Headers["X-Test-Auth-Failure"] =
                $"{context.Exception.GetType().Name}: {context.Exception.Message}";

            return Task.CompletedTask;
        };

        options.TokenValidationParameters.IssuerSigningKey =
           new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(context.Configuration["Jwt:Key"]!));

        options.TokenValidationParameters.ValidAudience =
            context.Configuration["Jwt:Audience"];

        options.TokenValidationParameters.ValidIssuer =
            context.Configuration["Jwt:Issuer"];

    });

        });


    }
}
