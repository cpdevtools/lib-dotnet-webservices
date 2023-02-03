
using System.Reflection;
using CpDevTools.Webservices.Exceptions;
using CpDevTools.Webservices.Models.Errors;
using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CpDevTools.Webservices.Extensions
{
  public static class MvcExtensions
  {

    public static IMvcBuilder SetupWebserviceMvc(this IServiceCollection serviceCollection, Action<MvcOptions, IConfiguration>? configureControllers = null, Action<MvcNewtonsoftJsonOptions, IConfiguration>? configureJson = null)
    {
      IMvcBuilder? mvcBuilder = null;
      ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
      {
        serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
        serviceCollection.AddHealthChecks();
        mvcBuilder = serviceCollection
                  .AddControllers(options =>
                  {
                    if (configureControllers != null)
                    {
                      configureControllers(options, cfg);
                    }
                  })
                  .AddMvcOptions(options =>
                  {
                    options.Filters.Add<HttpResponseExceptionFilter>();
                  })
                  .ConfigureApiBehaviorOptions(options =>
                  {
                    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(new ValidationErrorModel
                    {
                      TraceId = context.HttpContext.TraceIdentifier,
                      Details = ValidationErrorsModel.FromModelState(context.ModelState)
                    });
                  })
                  .AddDataAnnotationsLocalization()
                  .AddApplicationPart(Assembly.GetEntryAssembly()!)
                  .AddNewtonsoftJson(options =>
                  {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                      NamingStrategy = new CamelCaseNamingStrategy()
                    });
                    if (configureJson != null)
                    {
                      configureJson(options, cfg);
                    }
                  });
      });
      return mvcBuilder!;
    }
    public static WebApplication UseWebserviceMvc(this WebApplication app)
    {
      app.UseRouting();
      app.MapControllers();
      return app;
    }
  }

}
