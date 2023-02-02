
using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using DotNet.Globbing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CpDevTools.Webservices.Extensions
{
  public static class ForwardedHeadersExtensions
  {

    public static WebApplication UseWebserviceForwardedHeaders(this WebApplication app)
    {
      var fordwardedHeaderOptions = new ForwardedHeadersOptions
      {
        ForwardedHeaders =
              ForwardedHeaders.XForwardedFor
              | ForwardedHeaders.XForwardedProto
              | ForwardedHeaders.XForwardedHost
      };
      fordwardedHeaderOptions.KnownNetworks.Clear();
      fordwardedHeaderOptions.KnownProxies.Clear();
      app.UseForwardedHeaders(fordwardedHeaderOptions);
      return app;
    }
  }

}
