

using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CpDevTools.Webservices.Configuration
{

    public class CorsConfiguration
    {

        public bool? Enabled { get; set; }
        public List<string> AllowedDomains { get; set; } = new();
        public List<string> DeniedDomains { get; set; } = new();


        public CorsConfiguration()
        {
            if (Enabled == null)
            {
                Enabled = false;
            }
        }
    }
}