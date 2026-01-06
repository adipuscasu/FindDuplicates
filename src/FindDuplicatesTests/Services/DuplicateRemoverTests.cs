using FindDuplicates.Services;
using FindDuplicates.Tests.Helpers;

namespace FindDuplicates.Tests.Services;

public class DuplicateRemoverTests
{

    [Fact]
    public void Execute_RemovesDuplicateFilesKeepingOnePerGroup()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            // Create duplicates: two files with identical content and one unique file
            var keep = Path.Combine(tempRoot, "keep.txt");
            var remove = Path.Combine(tempRoot, "remove.txt");
            var unique = Path.Combine(tempRoot, "unique.txt");

            File.WriteAllText(keep, "same content");
            File.WriteAllText(remove, "same content");
            File.WriteAllText(unique, "unique content");

            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());
            var remover = new DuplicateRemover(finder, new FakeLogger<DuplicateRemover>());

            // Act
            remover.Execute(tempRoot);

            // Assert
            // The unique file must still exist
            Assert.True(File.Exists(unique), "Unique file should remain.");

            // Among the two identical files, exactly one must remain
            var remainingSameFiles = Directory.GetFiles(tempRoot)
                .Where(p => Path.GetFileName(p).Equals("keep.txt", StringComparison.OrdinalIgnoreCase)
                         || Path.GetFileName(p).Equals("remove.txt", StringComparison.OrdinalIgnoreCase))
                .ToList();

            Assert.Single(remainingSameFiles);
            // Ensure the remaining file contains the expected content
            Assert.Equal("same content", File.ReadAllText(remainingSameFiles.Single()));
        }
        finally
        {
            // Cleanup
            Directory.Delete(tempRoot, true);
        }
    }
}