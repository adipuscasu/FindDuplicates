using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace FindDuplicates.Services;

public class CommandHandler : ICommandHandler
{
    private readonly Dictionary<string, IFileProcessor> _processors;
    private readonly IUsageDisplay _usageDisplay;
    private readonly ILogger<CommandHandler> _logger;
    private readonly IFileFilter? _fileFilter;

    public CommandHandler(
        DuplicateDisplayer displayProcessor,
        DuplicateRemover removeProcessor,
        IUsageDisplay usageDisplay,
        ILogger<CommandHandler> logger)
        : this(displayProcessor, removeProcessor, usageDisplay, logger, fileFilter: null)
    {
    }

    /// <summary>
    /// Creates a new instance with an optional file filter.
    /// </summary>
    public CommandHandler(
        DuplicateDisplayer displayProcessor,
        DuplicateRemover removeProcessor,
        IUsageDisplay usageDisplay,
        ILogger<CommandHandler> logger,
        IFileFilter? fileFilter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _usageDisplay = usageDisplay ?? throw new ArgumentNullException(nameof(usageDisplay));
        _fileFilter = fileFilter;

        _processors = new Dictionary<string, IFileProcessor>(StringComparer.OrdinalIgnoreCase)
        {
            { "find", displayProcessor ?? throw new ArgumentNullException(nameof(displayProcessor)) },
            { "remove", removeProcessor ?? throw new ArgumentNullException(nameof(removeProcessor)) }
        };
    }

    public void HandleCommand(string[] args)
    {
        if (args is null || args.Length == 0)
        {
            _usageDisplay.ShowUsage();
            return;
        }

        string command = args[0].ToLowerInvariant();

        // Determine folder path as the first non-flag argument after the command.
        string folderPath = args.Skip(1).FirstOrDefault(a => !a.StartsWith("-")) ?? Directory.GetCurrentDirectory();

        if (!Directory.Exists(folderPath))
        {
            _logger.LogError("Error: Directory '{FolderPath}' does not exist.", folderPath);
            return;
        }

        if (string.Equals(command, "help", StringComparison.OrdinalIgnoreCase))
        {
            _usageDisplay.ShowUsage();
            return;
        }

        // Parse --extensions / -e flag from remaining args
        var fileFilter = ParseExtensionsFlag(args.Skip(1).ToArray(), _logger);

        // Only call the remove processor when explicitly requested via flag.
        if (string.Equals(command, "find", StringComparison.OrdinalIgnoreCase))
        {
            bool removeRequested = args
                .Skip(1)
                .Any(a => a.Equals("--remove", StringComparison.OrdinalIgnoreCase) || a.Equals("-r", StringComparison.OrdinalIgnoreCase));

            if (removeRequested)
            {
                if (!_processors.TryGetValue("remove", out var removeProcessor))
                {
                    _logger.LogError("Remove operation not available.");
                    _usageDisplay.ShowUsage();
                    return;
                }

                removeProcessor.Execute(folderPath, fileFilter);
                return;
            }
        }

        if (!_processors.TryGetValue(command, out var processor))
        {
            _logger.LogWarning("Unknown command: {Command}", command);
            _usageDisplay.ShowUsage();
            return;
        }

        processor.Execute(folderPath, fileFilter);
    }

    /// <summary>
    /// Parses --extensions or -e flag from the argument array.
    /// Returns null if no extension flag is found or if the flag is invalid.
    /// </summary>
    internal static IFileFilter? ParseExtensionsFlag(string[] args, ILogger? logger = null)
    {
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            var lower = arg.ToLowerInvariant();

            if (lower == "--extensions" || lower == "-e")
            {
                // Check if the next arg exists and is not a flag
                if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                {
                    return ExtensionFileFilter.FromCommaSeparated(args[i + 1]);
                }

                logger?.LogWarning("Extension flag '{Flag}' requires a value.", arg);
                return null;
            }
        }

        return null;
    }
}
