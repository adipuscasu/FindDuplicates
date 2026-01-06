using FindDuplicates.Services;
using FindDuplicates.Tests.Helpers;

namespace FindDuplicates.Tests.Services;

public class DuplicateFinderTests
{
    [Fact]
    public void GetDuplicateGroups_ReturnsGroupsForIdenticalFiles()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var fileA1 = Path.Combine(tempRoot, "a1.txt");
            var fileA2 = Path.Combine(tempRoot, "a2.txt");
            var fileB = Path.Combine(tempRoot, "b.txt");

            File.WriteAllText(fileA1, "duplicate content");
            File.WriteAllText(fileA2, "duplicate content");
            File.WriteAllText(fileB, "different content");

            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());

            // Act
            var groups = finder.GetDuplicateGroups(tempRoot);

            // Assert
            Assert.NotNull(groups);
            // There should be at least one group (the duplicate "a" files).
            var duplicateGroup = groups.Values.FirstOrDefault(g => g.Files.Count > 1);
            Assert.NotNull(duplicateGroup);
            Assert.Contains(fileA1, duplicateGroup.Files);
            Assert.Contains(fileA2, duplicateGroup.Files);
            Assert.DoesNotContain(fileB, duplicateGroup.Files);
        }
        finally
        {
            // Cleanup
            Directory.Delete(tempRoot, true);
        }
    }
}