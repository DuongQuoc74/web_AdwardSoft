using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AdwardSoft.Web.Inside
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.logs.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(appSettings)
                //        .MinimumLevel.Debug()
                //        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                //        .Enrich.FromLogContext()
                //        .WriteTo.Seq("http://localhost:5341")
                //        .WriteTo.Console()
                //        .WriteTo.MSSqlServer(connectionString: @"", sinkOptions: new SinkOptions { TableName = "LogEvents" }, null, appSettings)
                //        .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day,
                //outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .UseSerilog()
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             });

    }
}
