using FindDuplicates.Services;
using FindDuplicates.Tests.Helpers;

namespace FindDuplicates.Tests;

public class DuplicateRemoverDeterministicTests
{

    [Fact]
    public void Execute_KeepsLexicographicallySmallestFile_RemovesOthers()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var fileA = Path.Combine(tempRoot, "a.txt"); // should be kept (lexicographically smallest)
            var fileB = Path.Combine(tempRoot, "b.txt"); // should be removed
            File.WriteAllText(fileA, "same content");
            File.WriteAllText(fileB, "same content");

            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());
            var remover = new DuplicateRemover(finder, new FakeLogger<DuplicateRemover>());

            // Act
            remover.Execute(tempRoot);

            // Assert
            Assert.True(File.Exists(fileA), "Expected the lexicographically smallest file to remain.");
            Assert.False(File.Exists(fileB), "Expected other duplicates to be removed.");
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }
}
