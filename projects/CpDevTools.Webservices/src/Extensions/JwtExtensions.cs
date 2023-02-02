
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CpDevTools.Webservices.Extensions
{
  public static class JwtExtensions
  {
    private static readonly string SCHEME_NAME = "Bearer";

    private static JwtConfiguration GetConfiguration(IConfiguration config)
    {
      return config.GetValue<JwtConfiguration>("security:jwt") ?? new JwtConfiguration();
    }

    public static IServiceCollection SetupWebserviceJwtAuthentication(this IServiceCollection serviceCollection)
    {
      ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
      {
        var config = GetConfiguration(cfg);
        serviceCollection
                 .AddAuthentication(SCHEME_NAME)
                 .AddJwtBearer(SCHEME_NAME, options =>
                 {
               options.Authority = config.Authority;
               if (!env.IsProduction())
               {
                 options.BackchannelHttpHandler = new HttpClientHandler
                 {
                   ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                 };
               }
               options.TokenValidationParameters = new TokenValidationParameters
               {
                 ValidateAudience = false
               };
             });

      });
      return serviceCollection;
    }
    public static WebApplication UseWebserviceJwtAuthentication(this WebApplication app)
    {
      app
          .UseAuthentication()
          .UseAuthorization();
      return app;
    }
  }

}
