using Microsoft.Extensions.Logging;

namespace FindDuplicates.Services;

public class UsageDisplay : IUsageDisplay
{
    private readonly ILogger<UsageDisplay> _logger;

    public UsageDisplay(ILogger<UsageDisplay> logger)
    {
        _logger = logger;
    }

    public void ShowUsage()
    {
        _logger.LogInformation("FindDuplicates - Find and remove duplicate files");
        _logger.LogInformation("Usage:");
        _logger.LogInformation("  dotnet run find [folder]     - Find duplicates in folder");
        _logger.LogInformation("  dotnet run remove [folder]   - Remove duplicate files from folder");
        _logger.LogInformation("  dotnet run help              - Show this help message");
        _logger.LogInformation("Examples:");
        _logger.LogInformation("  dotnet run find \"C:\\Music\"");
        _logger.LogInformation("  dotnet run remove \"C:\\EBooks\"");
    }
}