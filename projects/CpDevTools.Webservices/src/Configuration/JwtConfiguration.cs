

using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CpDevTools.Webservices.Configuration
{

    public class JwtConfiguration
    {

        public bool? Enabled { get; set; }
        public string? Authority { get; set; }


        public JwtConfiguration()
        {
            if (Enabled == null)
            {
                Enabled = false;
            }
        }
    }
}