
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CpDevTools.Webservices.Util
{
  public static class ExtensionUtil
  {
    public static void Config(IServiceCollection serviceCollection, Action<IConfiguration, IWebHostEnvironment, IServiceProvider?> action)
    {
      using (var scope = serviceCollection.BuildServiceProvider().CreateScope()!)
      {
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();
        var env = services.GetRequiredService<IWebHostEnvironment>();
        action(config, env, services);
      }
    }

    public static void Config(WebApplication app, Action<IConfiguration, IWebHostEnvironment, IServiceProvider> action)
    {
      using (var scope = app.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();
        var env = services.GetRequiredService<IWebHostEnvironment>();
        action(config, env, services);
      }
    }

  }

}
