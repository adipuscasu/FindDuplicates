using System;
using FindDuplicates.Models;
using FindDuplicates.Utilities;
using Microsoft.Extensions.Logging;

namespace FindDuplicates.Services;

public class DuplicateDisplayer : IFileProcessor
{
    private const string _removeDuplicatesInstructions = "To remove duplicates, run: dotnet run remove \"{RootPath}\"  OR  dotnet run find --remove \"{RootPath}\"";
    private readonly IDuplicateFinder _duplicateFinder;
    private readonly ILogger<DuplicateDisplayer> _logger;

    public DuplicateDisplayer(IDuplicateFinder duplicateFinder, ILogger<DuplicateDisplayer> logger)
    {
        _duplicateFinder = duplicateFinder ?? throw new ArgumentNullException(nameof(duplicateFinder));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Execute(string rootPath)
    {
        _logger.LogInformation("Scanning folder: {RootPath}", rootPath);
        _logger.LogInformation("Computing file hashes...");

        var duplicates = _duplicateFinder.GetDuplicateGroups(rootPath);

        if (duplicates.Count == 0)
        {
            _logger.LogInformation("No duplicates found.");
            return;
        }

        _logger.LogInformation("Found {Count} group(s) of duplicates.", duplicates.Count);

        int groupNumber = 1;
        long totalWastedSpace = 0;

        foreach (var group in duplicates.Values)
        {
            long wastedSpace = (group.Files.Count - 1) * group.FileSize;
            totalWastedSpace += wastedSpace;

            _logger.LogInformation("Group {GroupNumber}: {Count} duplicates ({FileSize} each, {Wasted} wasted)",
                groupNumber, group.Files.Count, FileFormatter.FormatBytes(group.FileSize), FileFormatter.FormatBytes(wastedSpace));

            foreach (var file in group.Files)
            {
                _logger.LogInformation("  - {FilePath}", file);
            }

            _logger.LogInformation(string.Empty);
            groupNumber++;
        }

        _logger.LogInformation("Total wasted space: {Total}", FileFormatter.FormatBytes(totalWastedSpace));
        // Template contains two {RootPath} placeholders; pass the value twice so structured logging matches tokens.
        _logger.LogInformation(_removeDuplicatesInstructions, rootPath, rootPath);
    }
}