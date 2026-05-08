using FindDuplicates.Models;

namespace FindDuplicates.Services;

using FindDuplicates.Services;

public interface IDuplicateFinder
{
    Dictionary<string, DuplicateGroup> GetDuplicateGroups(string rootPath, IFileFilter? fileFilter = null);
}