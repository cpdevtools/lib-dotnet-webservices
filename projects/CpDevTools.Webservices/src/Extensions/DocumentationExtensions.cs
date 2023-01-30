
using CpDevTools.Webservices.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CpDevTools.Webservices.Extensions
{
    public static class DocumentationExtensions
    {
        private static DocumentationConfiguration GetConfiguration(IConfiguration config)
        {
            return config.GetValue<DocumentationConfiguration>("documentation") ?? new DocumentationConfiguration();
        }

        public static IServiceCollection SetupWebserviceDocumentation(this IServiceCollection serviceCollection)
        {
            ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
            {
                var config = GetConfiguration(cfg);

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                serviceCollection.AddEndpointsApiExplorer();
                serviceCollection.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(config.Name, config);
                });
            });
            return serviceCollection;
        }
        public static WebApplication UseWebserviceDocumentation(this WebApplication app)
        {
            ExtensionUtil.Config(app, (cfg, env, services) =>
            {
                var config = GetConfiguration(cfg);
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "documentation/api/{documentName}.specification.{json|yaml|yml}";
                });
                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "documentation";
                    options.SwaggerEndpoint($"api/{config.Name}.specification.yml", "Yaml");
                    options.SwaggerEndpoint($"api/{config.Name}.specification.json", "Json");
                });

            });
            return app;
        }
    }

}