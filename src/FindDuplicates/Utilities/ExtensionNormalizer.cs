namespace FindDuplicates.Utilities;

/// <summary>
/// Provides normalization of file extension strings.
/// Strips leading dots and lowercases the result.
/// </summary>
public static class ExtensionNormalizer
{
    /// <summary>
    /// Normalizes a file extension by stripping any leading dots and lowercasing.
    /// For example: ".JPG" or "JPG" → ".jpg".
    /// </summary>
    /// <param name="extension">The raw extension string.</param>
    /// <returns>A normalized extension string starting with a dot, e.g. ".jpg".
    /// Returns ".all" if the input is null, empty, or whitespace.</returns>
    public static string Normalize(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return ".all";

        var trimmed = extension.Trim();
        while (trimmed.StartsWith(".", StringComparison.Ordinal))
            trimmed = trimmed.Substring(1);

        return $".{trimmed.ToLowerInvariant()}";
    }

    /// <summary>
    /// Normalizes a list of extension strings.
    /// </summary>
    /// <param name="extensions">The raw extensions to normalize.</param>
    /// <returns>A HashSet of normalized extensions for fast lookups.</returns>
    public static HashSet<string> NormalizeMany(IEnumerable<string> extensions)
    {
        return new HashSet<string>(
            extensions.Select(e => Normalize(e)),
            StringComparer.OrdinalIgnoreCase);
    }
}
