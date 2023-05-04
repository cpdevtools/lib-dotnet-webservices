using System.Globalization;
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace CpDevTools.Webservices.Extensions
{
  public static class ApiInfoExtensions
  {
    public static WebApplication AddApiInfoToHeaders(this WebApplication app)
    {
      ExtensionUtil.Config(app, (config, env, services) =>
      {
        var cfg = ConfigUtil.GetConfig<WebserviceConfiguration>(config, "webservice") ?? new WebserviceConfiguration();
        app.Use((context, next) =>
        {
          context.Response.Headers.Add("X-Api-Id", cfg.DocumentId);
          context.Response.Headers.Add("X-Api-Name", cfg.Title);
          context.Response.Headers.Add("X-Api-Version", cfg.Version);
          context.Response.Headers.Add("X-Api-Company", cfg.Company);
          context.Response.Headers.Add("X-Api-LicenseName", cfg.License?.Name);
          context.Response.Headers.Add("X-Api-LicenseUrl", cfg.License?.Url);
          if (!env.IsProduction())
          {
            context.Response.Headers.Add("X-Api-BuildType", cfg.BuildType);
            context.Response.Headers.Add("X-Api-BuiltOn", cfg.BuiltOn?.ToString("o", CultureInfo.InvariantCulture));
            context.Response.Headers.Add("X-Api-CommitSha", cfg.CommitSha);
            context.Response.Headers.Add("X-Api-Environment", cfg.Environment);
          }
          return next(context);
        });
      });

      return app;
    }
  }

}
