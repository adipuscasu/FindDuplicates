namespace FindDuplicates.Services;

/// <summary>
/// Defines a contract for processing files (displaying or removing duplicates).
/// </summary>
public interface IFileProcessor
{
    /// <summary>
    /// Executes the processor on the given root path.
    /// </summary>
    /// <param name="rootPath">The directory to process.</param>
    /// <param name="fileFilter">Optional filter to apply before processing.</param>
    void Execute(string rootPath, IFileFilter? fileFilter = null);
}
