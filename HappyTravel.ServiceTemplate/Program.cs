using System.Threading.Tasks;
using HappyTravel.ServiceTemplate.Infrastructure.Environments;
using HappyTravel.StdOutLogger.Extensions;
using HappyTravel.StdOutLogger.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HappyTravel.ServiceTemplate
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environment = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders()
                        .AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

                    var env = hostingContext.HostingEnvironment;
                    if (env.IsLocal())
                        logging.AddConsole();
                    else
                        logging
                            .AddStdOutLogger(options =>
                            {
                                options.IncludeScopes = false;
                                options.RequestIdHeader = Constants.DefaultRequestIdHeader;
                                options.UseUtcTimestamp = true;
                            })
                            .AddSentry(options => { options.Dsn = EnvironmentVariableHelper.Get("Logging:Sentry:Endpoint", hostingContext.Configuration); });
                })
                .UseSetting(WebHostDefaults.SuppressStatusMessagesKey, "true");
    }
}
