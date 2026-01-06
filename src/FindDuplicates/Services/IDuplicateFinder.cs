using FindDuplicates.Models;

namespace FindDuplicates.Services;

public interface IDuplicateFinder
{
    Dictionary<string, DuplicateGroup> GetDuplicateGroups(string rootPath);
}