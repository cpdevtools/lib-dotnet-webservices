

using CpDevTools.Webservices.Configuration;
using CpDevTools.Webservices.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CpDevTools.Webservices.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddWebserviceDatabase<TContext>(this IServiceCollection serviceCollection, string connectionName) where TContext : DbContext
        {
            ExtensionUtil.Config(serviceCollection, (cfg, env, services) =>
            { 
                serviceCollection.TryAddSingleton<ConnectionConfiguration<TContext>>(
                    providers => ConnectionConfiguration<TContext>.FromConfigurationByName(cfg, connectionName)
                );
                serviceCollection.AddDbContext<TContext>();
            });
            return serviceCollection;
        }

    }

}