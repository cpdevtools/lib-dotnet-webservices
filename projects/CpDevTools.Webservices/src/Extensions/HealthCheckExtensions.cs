using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CpDevTools.Webservices.Extensions
{
  public static class HealthCheckExtensions
  {

    public static IServiceCollection SetupWebserviceHealthCheck(this IServiceCollection serviceCollection, Action<MvcOptions, IConfiguration>? configureControllers = null, Action<MvcNewtonsoftJsonOptions, IConfiguration>? configureJson = null)
    {
      serviceCollection.AddHealthChecks();
      return serviceCollection;

    }
    public static WebApplication UseWebserviceHealthCheck(this WebApplication app)
    {
      app.UseHealthChecks("/healthcheck");

      return app;
    }
  }

}
