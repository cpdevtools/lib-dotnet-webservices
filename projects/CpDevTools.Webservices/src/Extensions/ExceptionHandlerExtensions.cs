using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CpDevTools.Webservices.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static IServiceCollection SetupWebserviceExceptionHandlers(this IServiceCollection serviceCollection)
        {
            ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
            {

            });
            return serviceCollection;
        }

        public static WebApplication UseWebserviceExceptionHandlers(this WebApplication app)
        {
            ExtensionUtil.Config(app, (cfg, env, services) =>
            {
                if (!env.IsProduction())
                {
                    app.UseDeveloperExceptionPage();
                }
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async (context) =>
                    {
                        if (context.Response.Headers.ContentType == "application/json")
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                        }
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var error = exceptionHandlerPathFeature?.Error;

                        if (error != null)
                        {
                            await context.Response.WriteAsync($"\"{error.Message}\"");

                        }
                        else
                        {
                            await context.Response.WriteAsync("\"Internal Server Error\"");
                        }
                    });
                });

            });
            return app;
        }
    }

}