

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CpDevTools.Webservices.Util
{
  public static class ConfigUtil
  {

    public static T? GetConfig<T>(IConfiguration config, string path)
    {
      return config.GetSection(path).Get<T>();
    }
    public static T? GetConfig<T>(IConfigurationSection config)
    {
      return config.Get<T>();
    }
  }

}
