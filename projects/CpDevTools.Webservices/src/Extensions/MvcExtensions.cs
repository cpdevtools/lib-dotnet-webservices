
using System.Reflection;
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using DotNet.Globbing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CpDevTools.Webservices.Extensions
{
    public static class MvcExtensions
    {
        private static CorsConfiguration GetConfiguration(IConfiguration config)
        {
            return config.GetValue<CorsConfiguration>("security:cors") ?? new CorsConfiguration();
        }



        public static IServiceCollection SetupWebserviceControllers(this IServiceCollection serviceCollection)
        {
            ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
            {
                var config = GetConfiguration(cfg);
                serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                var a = serviceCollection
                    .AddControllers(options =>
                    {

                    })
                    .AddApplicationPart(Assembly.GetEntryAssembly()!)
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        });
                    });
            });
            return serviceCollection;
        }
        public static WebApplication UseWebserviceControllers(this WebApplication app)
        {
            app.UseRouting();
            app.MapControllers();
            return app;
        }
    }

}