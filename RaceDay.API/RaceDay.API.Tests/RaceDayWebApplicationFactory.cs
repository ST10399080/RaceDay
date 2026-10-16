using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RaceDay.API.Data;

namespace RaceDay.API.Tests;

public class RaceDayWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<
                DbContextOptions<RaceDayDbContext>>();

            services.RemoveAll<RaceDayDbContext>();

            services.AddDbContext<RaceDayDbContext>(
                options => options.UseInMemoryDatabase(
                    "RaceDayIntegrationTests"));
        });
    }
}
