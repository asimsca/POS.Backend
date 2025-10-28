using Serilog.Events;
using Serilog;

namespace POS.Backend.Helper.Logging
{
    public static class SerilogConfiguration
    {
        public static void ConfigureLogging()
        {
            var logDirectory = "Logs"; // Set your desired log directory
            var logFileName = Path.Combine(logDirectory, $"log-{DateTime.Now:dd-MM-yyyy}.txt");

            // Set up Serilog as the logging provider with custom configuration
            Log.Logger = new LoggerConfiguration()
                // Set default minimum level for application's logs (includes Debug and up)
                .MinimumLevel.Information()

                // Override logging for Microsoft namespaces to reduce log noise
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)       // Log only warnings or above
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)          // Reduce noise from .NET system logs
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // Reduce ASP.NET Core-specific logs

                // Write logs to a rolling log file
                .WriteTo.File(
                    path: logFileName,                   // File path and naming
            rollingInterval: RollingInterval.Infinite,    // Create a new file each day
                    rollOnFileSizeLimit: true,               // Roll the file if it exceeds size limit
                    fileSizeLimitBytes: 5 * 1024 * 1024,     // Max size = 5MB
                    retainedFileCountLimit: null,              // Keep only the latest 10 log files
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}" // Format
                )

                // Finalize the logger setup
                .CreateLogger();
        }
    }
}
