namespace FindDuplicates.Models;

public class DuplicateGroup
{
    public string Hash { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public List<string> Files { get; set; } = new();
}