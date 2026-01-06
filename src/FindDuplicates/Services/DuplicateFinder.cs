using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FindDuplicates.Models;
using FindDuplicates.Utilities;
using Microsoft.Extensions.Logging;

namespace FindDuplicates.Services;

public class DuplicateFinder : IDuplicateFinder
{
    private readonly ILogger<DuplicateFinder> _logger;

    public DuplicateFinder(ILogger<DuplicateFinder> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Dictionary<string, DuplicateGroup> GetDuplicateGroups(string rootPath)
    {
        var fileHashes = new Dictionary<string, DuplicateGroup>();

        var files = Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories)
            .Where(f => !FileHasher.IsSystemFile(f))
            .ToList();

        int totalFiles = files.Count;
        if (totalFiles == 0)
        {
            _logger.LogInformation("Processed: 0 files total");
            return new Dictionary<string, DuplicateGroup>();
        }

        int processedCount = 0;
        int nextReportPercent = 10; // Report at 10%, 20%, ... 100%

        foreach (var file in files)
        {
            try
            {
                var fileInfo = new FileInfo(file);
                string hash = FileHasher.ComputeFileHash(file);

                if (fileHashes.ContainsKey(hash))
                {
                    fileHashes[hash].Files.Add(file);
                }
                else
                {
                    fileHashes[hash] = new DuplicateGroup
                    {
                        Hash = hash,
                        FileSize = fileInfo.Length,
                        Files = new List<string> { file }
                    };
                }

                processedCount++;

                // Calculate progress percentage and emit reports for each 10% threshold crossed.
                int percent = (int)((processedCount * 100L) / totalFiles);
                while (percent >= nextReportPercent && nextReportPercent <= 100)
                {
                    _logger.LogInformation("Progress: {Percent}% ({Processed}/{Total})", nextReportPercent, processedCount, totalFiles);
                    nextReportPercent += 10;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing {File}", file);
            }
        }

        // Ensure final 100% report is written if not already emitted
        if (nextReportPercent <= 100)
        {
            _logger.LogInformation("Progress: {Percent}% ({Processed}/{Total})", 100, processedCount, totalFiles);
        }

        _logger.LogInformation("Processed: {Processed} files total", processedCount);

        // Return only groups with duplicates
        return fileHashes.Where(kvp => kvp.Value.Files.Count > 1)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}