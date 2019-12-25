using System;
using HappyTravel.ServiceTemplate.Services;
using HappyTravel.VaultClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HappyTravel.ServiceTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services, ILoggerFactory loggerFactory)
        {
            var serializationSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            JsonConvert.DefaultSettings = () => serializationSettings;

            using var vault = new VaultClient.VaultClient(new VaultOptions
            {
                BaseUrl = new Uri(Configuration["Vault:Endpoint"]),
                Engine = Configuration["Vault:Engine"],
                Role = Configuration["Vault:Role"]
            }, loggerFactory);

            services.AddHttpClient();
            services.AddHealthChecks();

            services.AddHostedService<Host>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/health");
        }


        private IConfiguration Configuration { get; }
    }
}
