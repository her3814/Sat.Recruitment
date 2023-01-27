using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyApi.Filters;
using Sat.Recruitment.Api.Configuration;
using Sat.Recruitment.Api.Filters;
using Sat.Recruitment.Api.Middlewares;
using Serilog;
using System;
using System.Text.Json.Serialization;

namespace Sat.Recruitment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddMvc().AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            //services.AddScoped<ValidateModelFilter>();
            services.RegisterRepositories();
            services.RegisterApplicationServices();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseRouting();

            app.UseAuthorization();

            //app.ConfigureMiddlewares();
            app.UseMiddleware<ResponseResultMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
