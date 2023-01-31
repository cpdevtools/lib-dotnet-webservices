
using System.Reflection;
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using DotNet.Globbing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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



        public static IMvcBuilder SetupWebserviceMvc(this IServiceCollection serviceCollection, Action<MvcOptions, IConfiguration>? configureControllers = null, Action<MvcNewtonsoftJsonOptions, IConfiguration>? configureJson = null)
        {
            IMvcBuilder? mvcBuilder = null;
            ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
            {

                var config = GetConfiguration(cfg);
                serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                mvcBuilder = serviceCollection
                    .AddControllers(options =>
                    {
                        if (configureControllers != null)
                        {
                            configureControllers(options, cfg);
                        }
                    })
                    .AddApplicationPart(Assembly.GetEntryAssembly()!)
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        });
                        if (configureJson != null)
                        {
                            configureJson(options, cfg);
                        }
                    });
            });
            return mvcBuilder!;
        }
        public static WebApplication UseWebserviceMvc(this WebApplication app)
        {
            app.UseRouting();
            app.MapControllers();
            return app;
        }
    }

}