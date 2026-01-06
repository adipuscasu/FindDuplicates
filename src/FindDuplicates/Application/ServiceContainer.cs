using FindDuplicates.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace FindDuplicates.Application;

public static class ServiceContainer
{
    public static App CreateApp()
    {
        // Build configuration (appsettings.json)
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Configure Serilog from configuration (fallback handled by Serilog itself)
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        // Setup DI
        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);

        // Register Serilog as logging provider for Microsoft.Extensions.Logging
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
        });

        // Register application services
        services.AddSingleton<IDuplicateFinder, DuplicateFinder>();
        services.AddSingleton<IUsageDisplay, UsageDisplay>();

        // File processors
        services.AddTransient<DuplicateDisplayer>();
        services.AddTransient<DuplicateRemover>();

        // Command handler and app
        services.AddTransient<IFileProcessor>(sp => sp.GetRequiredService<DuplicateDisplayer>());
        services.AddTransient<IFileProcessor>(sp => sp.GetRequiredService<DuplicateRemover>());
        services.AddTransient<ICommandHandler, CommandHandler>();
        services.AddTransient<App>();

        var provider = services.BuildServiceProvider();

        // Return App resolved from DI container
        return provider.GetRequiredService<App>();
    }
}