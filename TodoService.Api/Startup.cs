using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TodoService.Api.Extensions;
using TodoService.Api.Options;
using TodoService.Core.Interfaces;
using TodoService.Infrastructure.Data;

namespace TodoService.Api
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
            // Bind database options. Invalid configuration will terminate the application startup.
            var connectionStringsOptions =
                Configuration.GetSection("ConnectionStrings").Get<ConnectionStringsOptions>();
            var cosmosDbOptions = Configuration.GetSection("CosmosDb").Get<CosmosDbOptions>();
            var (serviceEndpoint, authKey) = connectionStringsOptions.ActiveConnectionStringOptions;
            var (databaseName, collectionData) = cosmosDbOptions;
            var collectionNames = collectionData.Select(c => c.Name).ToList();

            // Add Mvc.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            // Add version support.
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            // Add CosmosDb. This verifies database and collections existence.
            services.AddCosmosDb(serviceEndpoint, authKey, databaseName, collectionNames);
            
            // Add health check by checking CosmosDb connection. Cache the result for 1 minute.
            services.AddHealthChecks(checks =>
            {
                checks.AddCosmosDbCheck(serviceEndpoint, authKey, TimeSpan.FromMinutes(1));
            });

            services.AddCustomSwagger();

            services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseAppInsightsLogger(loggerFactory);
                app.UseGlobalErrorHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
