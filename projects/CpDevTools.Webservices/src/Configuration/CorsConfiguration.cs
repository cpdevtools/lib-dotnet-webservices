

using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CpDevTools.Webservices.Configuration
{

  public class CorsConfiguration
  {

    public bool? Enabled { get; set; } = false;
    public List<string> AllowedDomains { get; set; } = new();
    public List<string> DeniedDomains { get; set; } = new();
  }
}
