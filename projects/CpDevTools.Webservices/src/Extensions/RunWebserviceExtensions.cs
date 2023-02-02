
using System.Reflection;
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CpDevTools.Webservices.Extensions
{
  public static class RunWebserviceExtensions
  {

    private static WebserviceConfiguration GetConfiguration(IConfiguration config)
    {
      return ConfigUtil.GetConfig<WebserviceConfiguration>(config, "webservice") ?? new WebserviceConfiguration();
    }
    public static void RunWebservice(this WebApplication app)
    {
      ExtensionUtil.Config(app, (cfg, env, services) =>
      {
        var config = GetConfiguration(cfg);
        var header = $"""


                    ======================================================================

                        Starting Webservice

                            {YamlUtil.Serialize(config).Replace(Environment.NewLine, Environment.NewLine + "        ")}
                    ======================================================================

                    """;

        LoggingUtil.Logger.Log(LogLevel.Information, header);
        app.Run();
      });
    }


  }

}
