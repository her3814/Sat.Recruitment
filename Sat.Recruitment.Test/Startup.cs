using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Test.Configuration;
using Serilog;
using System;
using System.Text.Json.Serialization;

namespace Sat.Recruitment.Test
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var logFile = Configuration.GetValue<string>("LogsFile");
            var log = new LoggerConfiguration()
                .WriteTo.File(logFile.Replace("{date}", DateTime.Today.ToString("yyyy-MM-dd")),
                              shared: true)
                .MinimumLevel.Error()
                .CreateLogger();

            services.AddLogging(c => c.ClearProviders().AddConsole().AddSerilog(log));
            services.AddSingleton(Log.Logger);

            services.RegisterApplicationServices();
            services.RegisterRepositories();
        }



        public void Configure(IApplicationBuilder app)
        {
        }

    }
}
