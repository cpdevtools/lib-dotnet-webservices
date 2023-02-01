using System.Reflection;
using Serilog;
using Serilog.Extensions.Logging;

namespace CpDevTools.Webservices.Util
{
    public static class LoggingUtil
    {
        private static bool _loggerInitialized;
        
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
        public static Microsoft.Extensions.Logging.ILogger Logger = new SerilogLoggerFactory(CreateDefaultLogger()).CreateLogger(Assembly.GetEntryAssembly()!.GetName().Name);


    }

}
