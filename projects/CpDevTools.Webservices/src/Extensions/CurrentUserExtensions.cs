
using CpDevTools.Webservices.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CpDevTools.Webservices.Extensions
{
  public static class CurrentUserExtensions
  {
    public static IServiceCollection AddWebserviceCurrentUserService<TService>(this IServiceCollection serviceCollection) where TService : ICurrentUserService<IUser>
    {
      serviceCollection.AddScoped(typeof(TService));
      serviceCollection.AddScoped<ICurrentUserService<IUser>>(providers => providers.GetRequiredService<TService>());

      var userType = typeof(TService).GetProperty(nameof(ICurrentUserService<IUser>.User))!.PropertyType;
      var serviceType = typeof(ICurrentUserService<>).MakeGenericType(userType);
      serviceCollection.AddScoped(serviceType, providers => providers.GetRequiredService<TService>());

      return serviceCollection;
    }
    public static IServiceCollection AddWebserviceCurrentUserService(this IServiceCollection serviceCollection)
    {
      return serviceCollection.AddWebserviceCurrentUserService<DefaultCurrentUserService>();
    }
  }
}
