using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Models.Db;
using CpDevTools.Webservices.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CpDevTools.Webservices.Extensions
{
  public static class DatabaseExtensions
  {

    private static HashSet<Type> _efContextTypes = new();

    public static IServiceCollection AddWebserviceDatabase<TContext>(this IServiceCollection serviceCollection, string connectionName) where TContext : DbContext
    {
      _efContextTypes.Add(typeof(TContext));
      ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
      {
        serviceCollection.TryAddSingleton<ConnectionConfiguration<TContext>>(
          providers => ConnectionConfiguration<TContext>.FromConfigurationByName(cfg, connectionName)
        );
        serviceCollection.AddDbContext<TContext>();
      });
      return serviceCollection;
    }

    public static WebApplication MigrateDatabases(this WebApplication app)
    {
      ExtensionUtil.Config(app, (cfg, env, services) =>
      {
        foreach (var cType in _efContextTypes)
        {
          var dbContext = services.GetService(cType);
          if (dbContext is CanMigrateDb db)
          {
            db.MigrateDb();
          }
        }
      });
      _efContextTypes = new();
      return app;
    }
  }
}
