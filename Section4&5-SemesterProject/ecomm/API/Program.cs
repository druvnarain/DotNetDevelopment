using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    //dotnet run executes Program's main method
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();         default

            var host = CreateHostBuilder(args).Build();

            //any code that runs inside is disposed of when methods inside are done
            //need to get access to the dbcontext outside of Startup.cs
            using (var scope = host.Services.CreateScope())
            {
                //Point to ConfigureServices in mentioned in Startup.cs to get services
                var services = scope.ServiceProvider;
                //used for logging info into console. Factory allows creation of Logger class
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();

                    //Apply any pending migrations to the database and creates database if it doesnt exist
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "Error has occurred during a migration");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            
            //Configures host(kestrel) for application
            //Reads from appsettings.json to configure
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
