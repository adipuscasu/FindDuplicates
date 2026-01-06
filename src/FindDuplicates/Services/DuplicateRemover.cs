using System;
using System.IO;
using System.Linq;
using FindDuplicates.Models;
using FindDuplicates.Utilities;
using Microsoft.Extensions.Logging;

namespace FindDuplicates.Services;

public class DuplicateRemover : IFileProcessor
{
    private readonly IDuplicateFinder _duplicateFinder;
    private readonly ILogger<DuplicateRemover> _logger;

    public DuplicateRemover(IDuplicateFinder duplicateFinder, ILogger<DuplicateRemover> logger)
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

        int filesRemoved = 0;
        long spaceFreed = 0;

        foreach (var group in duplicates.Values)
        {
            // Keep decision deterministic: sort paths using ordinal comparison
            var sortedFiles = group.Files.OrderBy(p => p, StringComparer.Ordinal).ToList();
            var filesToRemove = sortedFiles.Skip(1).ToList();

            foreach (var fileToRemove in filesToRemove)
            {
                try
                {
                    _logger.LogInformation("Removing: {File}", fileToRemove);
                    File.Delete(fileToRemove);
                    filesRemoved++;
                    spaceFreed += group.FileSize;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removing {File}", fileToRemove);
                }
            }
        }

        _logger.LogInformation("Removed {Count} duplicate file(s). Space freed: {Space}", filesRemoved, FileFormatter.FormatBytes(spaceFreed));
    }
}