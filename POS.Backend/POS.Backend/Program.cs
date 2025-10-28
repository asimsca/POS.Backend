using POS.Backend.Helper.Logging;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using POS.Backend;

SerilogConfiguration.ConfigureLogging();

try
{
    Log.Information("Starting POS web app...");

    var builder = WebApplication.CreateBuilder(args);

    // Telling .NET to use Serilog
    builder.Host.UseSerilog();

    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();
    startup.Configure(app, app.Environment);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "App startup failed");
}
finally
{
    Log.CloseAndFlush(); // Ensures logs are written on shutdown
}

