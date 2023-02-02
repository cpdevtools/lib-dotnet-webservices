

using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CpDevTools.Webservices.Configuration
{

  public class LoggingConfiguration
  {
    public class ElkConfiguration
    {
      public bool? Enabled { get; set; } = false;
      public List<string> Urls { get; set; } = new();

      public string? ApiKey { get; set; }
      public string? Username { get; set; }
      public string? Password { get; set; }
    }

    public Dictionary<string, string> LogLevel { get; set; } = new();
    public ElkConfiguration Elk { get; set; } = new();

  }
}
