
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CpDevTools.Webservices.Util
{
    public static class LoggingUtil
    {
        private static bool _loggerInitialized;
        private static Serilog.ILogger? _defaultLogger;

        private static Serilog.ILogger CreateDefaultLogger()
        {
            if (!_loggerInitialized)
            {
                _loggerInitialized = true;
                Serilog.Debugging.SelfLog.Enable(message => Console.WriteLine(message));
                var config = ApplyDefaults(new LoggerConfiguration());
                Log.Logger = config.CreateBootstrapLogger();
            }
            return Log.Logger;
        }

        public static LoggerConfiguration ApplyDefaults(LoggerConfiguration config)
        {
            return config
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console();
        }
        public static Microsoft.Extensions.Logging.ILogger Logger => (Microsoft.Extensions.Logging.ILogger)CreateDefaultLogger();

        
    }

}
