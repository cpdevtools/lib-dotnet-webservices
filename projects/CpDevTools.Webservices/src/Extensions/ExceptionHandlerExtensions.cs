using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CpDevTools.Webservices.Extensions
{
  public static class ExceptionHandlerExtensions
  {
    public static IServiceCollection SetupWebserviceExceptionHandlers(this IServiceCollection serviceCollection)
    {
      ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
      {

      });
      return serviceCollection;
    }

    public static WebApplication UseWebserviceExceptionHandlers(this WebApplication app)
    {
      ExtensionUtil.Config(app, (cfg, env, services) =>
      {
        if (!env.IsProduction())
        {
          app.UseDeveloperExceptionPage();
        }
      });
      return app;
    }
  }

}
