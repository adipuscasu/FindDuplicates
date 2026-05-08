using System;
using System.Collections.Generic;
using System.IO;
using FindDuplicates.Utilities;

namespace FindDuplicates.Services;

/// <summary>
/// Filters files based on their extension.
/// Follows the Single Responsibility Principle — its only job is extension matching.
/// </summary>
public class ExtensionFileFilter : IFileFilter
{
    private readonly HashSet<string> _extensions;

    /// <summary>
    /// Creates a new instance from a comma-separated list of extensions.
    /// Extensions may optionally start with a dot. Leading dots and whitespace are trimmed.
    /// Case is normalized to lowercase.
    /// </summary>
    /// <param name="extensions">Comma-separated extensions, e.g. "jpg,png,gif" or ".jpg, .png".</param>
    public static ExtensionFileFilter FromCommaSeparated(string extensions)
    {
        if (string.IsNullOrWhiteSpace(extensions))
            throw new ArgumentException("Extensions cannot be null or empty.", nameof(extensions));

        var parts = extensions.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 0)
            throw new ArgumentException("No valid extensions provided.", nameof(extensions));

        var normalized = ExtensionNormalizer.NormalizeMany(parts);
        return new ExtensionFileFilter(normalized);
    }

    /// <summary>
    /// Creates a new instance from an array of extensions.
    /// </summary>
    /// <param name="extensions">Extensions such as "jpg", "png" (with or without dots).</param>
    public static ExtensionFileFilter FromArray(params string[] extensions)
    {
        if (extensions is null || extensions.Length == 0)
            throw new ArgumentException("At least one extension must be provided.", nameof(extensions));

        var normalized = ExtensionNormalizer.NormalizeMany(extensions);
        return new ExtensionFileFilter(normalized);
    }

    private ExtensionFileFilter(HashSet<string> extensions)
    {
        _extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
    }

    /// <summary>
    /// Determines whether the given file path matches any of the configured extensions.
    /// </summary>
    public bool Matches(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        var ext = Path.GetExtension(filePath);
        return _extensions.Contains(ext);
    }
}
