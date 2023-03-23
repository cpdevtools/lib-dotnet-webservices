using Mcrio.Configuration.Provider.Docker.Secrets;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace CpDevTools.Webservices.Extensions
{
  public static class ConfigurationExtensions
  {
    public static IConfigurationBuilder SetupDockerWebserviceConfiguration(this ConfigurationManager cm, Action<IConfigurationBuilder>? configureBuilder = null)
    {
      cm.Sources.Clear();
      IConfigurationBuilder builder = cm
        .AddYamlFile("config.yml")
        .AddYamlFile("/config/environment.yml", true)
        .AddYamlFile("/config/webservice.yml", true)
        .AddYamlFile("build._meta_.yml", true);

      if (configureBuilder != null)
      {
        configureBuilder(builder);
      }

      return builder
        .AddEnvironmentVariables()
        .AddDockerSecrets()
        .AddCommandLine(Environment.GetCommandLineArgs());
    }
  }

}
