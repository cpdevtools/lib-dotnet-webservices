
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CpDevTools.Webservices.Extensions
{
    public static class DocumentationExtensions
    {
        private static DocumentationConfiguration GetConfiguration(IConfiguration config)
        {
            return ConfigUtil.GetConfig<DocumentationConfiguration>(config, "documentation") ?? new DocumentationConfiguration();
        }
        private static WebserviceConfiguration GetWebserviceConfiguration(IConfiguration config)
        {
            return ConfigUtil.GetConfig<WebserviceConfiguration>(config, "webservice") ?? new WebserviceConfiguration();
        }

        public static IServiceCollection SetupWebserviceDocumentation(this IServiceCollection serviceCollection)
        {
            ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
            {
                var serviceConfig = GetWebserviceConfiguration(cfg);
                var config = GetConfiguration(cfg);
                if (config.Enabled == true)
                {
                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    serviceCollection.AddEndpointsApiExplorer();
                    serviceCollection.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc(serviceConfig.DocumentId, serviceConfig);
                    });
                }

            });
            return serviceCollection;
        }
        public static WebApplication UseWebserviceDocumentation(this WebApplication app)
        {
            ExtensionUtil.Config(app, (cfg, env, services) =>
            {
                var config = GetConfiguration(cfg);
                if (config.Enabled == true)
                {
                    var serviceConfig = GetWebserviceConfiguration(cfg);
                    app.UseSwagger(options =>
                    {
                        options.RouteTemplate = "documentation/api/{documentName}.specification.{json|yaml|yml}";
                    });
                    app.UseSwaggerUI(options =>
                    {
                        options.RoutePrefix = "documentation";
                        options.SwaggerEndpoint($"api/{serviceConfig.DocumentId}.specification.yml", "Yaml");
                        options.SwaggerEndpoint($"api/{serviceConfig.DocumentId}.specification.json", "Json");
                    });
                }

            });
            return app;
        }
    }

}