

using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CpDevTools.Webservices.Configuration
{

  public class JwtConfiguration
  {

    public bool? Enabled { get; set; } = false;
    public string? Authority { get; set; }
  }
}
