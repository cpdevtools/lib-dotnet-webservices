
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using DotNet.Globbing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CpDevTools.Webservices.Extensions
{
  public static class CorsExtensions
  {
    private static CorsConfiguration GetConfiguration(IConfiguration config)
    {
      return ConfigUtil.GetConfig<CorsConfiguration>(config, "security:cors") ?? new CorsConfiguration();
    }

    private static string SwapSlashesAndDots(string source)
    {
      return source.Replace('/', '|').Replace('.', '/').Replace('|', '.');
    }

    public static IServiceCollection SetupWebserviceCors(this IServiceCollection serviceCollection)
    {
      ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
      {
        var config = GetConfiguration(cfg);
        var allowedOrigins = config.AllowedDomains.Select(p => Glob.Parse(SwapSlashesAndDots(p)));
        var deniedOrigins = config.DeniedDomains.Select(p => Glob.Parse(SwapSlashesAndDots(p)));

        serviceCollection.AddCors(corsOpts =>
              {
                corsOpts.AddDefaultPolicy(policyOpts =>
                      {
                        if (config.Enabled != true)
                        {
                          policyOpts.SetIsOriginAllowed(origin => true);
                        }
                        else
                        {
                          policyOpts.SetIsOriginAllowed(origin =>
                            {
                              if (origin.Contains("://"))
                              {
                                origin = origin.Split("://")[1];
                              }
                              origin = SwapSlashesAndDots(origin);
                              var allowed =
                                    !deniedOrigins.Where(g => g.IsMatch(origin)).Any() &&
                                    allowedOrigins.Where(g => g.IsMatch(origin)).Any();
                              return allowed;
                            });
                        }
                        policyOpts
                              .AllowCredentials()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .Build();
                      });
              });


      });
      return serviceCollection;
    }
    public static WebApplication UseWebserviceCors(this WebApplication app)
    {
      app.UseCors();
      return app;
    }
  }

}
