using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TaskManagementSystem.StatupExtentions
{
    public static class LoggingExtension
    {
        public static void AddSerilogLogging(this ConfigureHostBuilder hostBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext() // optional: adds useful context like RequestId
                .CreateLogger();

            hostBuilder.UseSerilog();
        }
    }
}
