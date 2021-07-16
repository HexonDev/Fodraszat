using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fodraszat.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
                var migrations = dbContext.Database.GetMigrations().ToHashSet();
                if ((await dbContext.Database.GetAppliedMigrationsAsync()).Any(a => !migrations.Contains(a)))
                    throw new InvalidOperationException("Az adatb�zison m�r van olyan migr�ci� futtatva, amit az�ta t�r�ltek a projektb�l. T�r�ld az adatb�zist vagy jav�tsd a migr�ci�k �llapot�t, majd ind�tsd �jra az alkalmaz�st!");
                await dbContext.Database.MigrateAsync();

                var roleSeeder = scope.ServiceProvider.GetRequiredService<IRoleSeedService>();
                await roleSeeder.SeedRoleAsync();

                var userSeeder = scope.ServiceProvider.GetRequiredService<IUserSeedService>();
                await userSeeder.SeedUserAsync();
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
