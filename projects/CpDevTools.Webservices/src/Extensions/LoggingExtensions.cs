
using System.Reflection;
using System.Security.Claims;
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace CpDevTools.Webservices.Extensions
{
  public static class LoggingExtensions
  {
    private static LoggingConfiguration GetConfiguration(IConfiguration config)
    {
      return config.GetValue<LoggingConfiguration>("logging") ?? new LoggingConfiguration();
    }

    public static Microsoft.Extensions.Logging.ILogger SetupWebserviceLogging(this WebApplicationBuilder webApplicationBuilder)
    {
      webApplicationBuilder.Host.UseSerilog((context, services, cfg) =>
      {
        var config = GetConfiguration(context.Configuration);
        var elkConfig = config.Elk;

        var logConfig = LoggingUtil.ApplyDefaults(cfg);

        if (elkConfig.Enabled ?? false)
        {
          logConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elkConfig.Urls.Select(url => new Uri(url)))
          {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            FailureCallback = e => Console.WriteLine($"Unable to submit event to elk '{e.MessageTemplate}'"),
            EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
            IndexFormat = $"logs-{Assembly.GetEntryAssembly()!.GetName().Name!.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
            TypeName = null,
            BatchAction = ElasticOpType.Create,
            ModifyConnectionSettings = c =>
                  {
                    if (!String.IsNullOrEmpty(elkConfig.ApiKey))
                    {
                      Console.WriteLine("Setting up Elasticsearch log sink with api key");
                      c = c.ApiKeyAuthentication(new ApiKeyAuthenticationCredentials(elkConfig.ApiKey));
                    }
                    else if (!String.IsNullOrEmpty(elkConfig.Username) && !String.IsNullOrEmpty(elkConfig.Password))
                    {
                      Console.WriteLine("Setting up Elasticsearch log sink with username and password");
                      c = c.BasicAuthentication(elkConfig.Username, elkConfig.Password);
                    }
                    return c;
                  }
          });
        }
      });
      return LoggingUtil.Logger;
    }
    public static WebApplication UseWebserviceLogging(this WebApplication app)
    {
      app.UseSerilogRequestLogging(options =>
      {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
              {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("RequestContentType", httpContext.Request.ContentType);
                diagnosticContext.Set("ResponseContentType", httpContext.Response.ContentType);
                diagnosticContext.Set("RequestContentLength", httpContext.Request.ContentLength);
                diagnosticContext.Set("ResponseContentLength", httpContext.Response.ContentLength);

                if (httpContext.User.Identity?.IsAuthenticated ?? false)
                {
                  diagnosticContext.Set("UserId", (httpContext.User.Identity as ClaimsIdentity)!.FindFirst("sub")?.Value);
                }
                try
                {
                  diagnosticContext.Set("SessionId", httpContext.Session.Id);
                }
                catch { }
              };
      });
      return app;
    }
  }

}
