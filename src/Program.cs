using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace SQ7MRU.QSOCollector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            logging.AddConsole();
                            logging.AddDebug();
                        }
                    }).ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        IHostingEnvironment env = hostingContext.HostingEnvironment;

                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                        config.AddEnvironmentVariables();
                        config.AddCommandLine(args);
                    })
                 .UseStartup<Startup>();
        
    }
}