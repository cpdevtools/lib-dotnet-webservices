using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CpDevTools.Webservices.Extensions
{
    public static class SameSiteCookieExtensions
    {
        public static IServiceCollection SetupWebserviceSameSiteCookies(this IServiceCollection serviceCollection)
        {
            ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
            {
                serviceCollection.Configure<CookiePolicyOptions>(CookieUtil.ConfigureSameSiteCookies);
            });
            return serviceCollection;
        }

        public static WebApplication UseWebserviceSameSiteCookies(this WebApplication app)
        {
            app.UseCookiePolicy();
            return app;
        }
    }

}