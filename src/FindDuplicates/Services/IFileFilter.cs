namespace FindDuplicates.Services;

/// <summary>
/// Defines a contract for filtering files before processing.
/// </summary>
public interface IFileFilter
{
    /// <summary>
    /// Determines whether the given file path passes the filter.
    /// </summary>
    /// <param name="filePath">The full path of the file to evaluate.</param>
    /// <returns><c>true</c> if the file matches the filter; otherwise, <c>false</c>.</returns>
    bool Matches(string filePath);
}
