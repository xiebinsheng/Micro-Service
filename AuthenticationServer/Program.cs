using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace AuthenticationServer.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //TODO: Temporary: it's not good to read appsettings.json here just to configure logging
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.WithProperty("Application", "AuthenticationServer")
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/logs.txt")
                //.WriteTo.Elasticsearch(
                //    new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]))
                //    {
                //        AutoRegisterTemplate = true,
                //        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                //        IndexFormat = "msdemo-log-{0:yyyy.MM}"
                //    })
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting AuthenticationServer.Host.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "AuthenticationServer.Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
