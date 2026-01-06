using FindDuplicates.Models;

namespace FindDuplicates.Tests.Models;

public class DuplicateGroupTests
{
    [Fact]
    public void DefaultProperties_AreInitializedCorrectly()
    {
        // Arrange & Act
        var group = new DuplicateGroup();

        // Assert
        Assert.Equal(string.Empty, group.Hash);
        Assert.Equal(0L, group.FileSize);
        Assert.NotNull(group.Files);
        Assert.IsType<List<string>>(group.Files);
        Assert.Empty(group.Files);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var files = new List<string> { "one.txt", "two.txt" };
        var group = new DuplicateGroup
        {
            Hash = "abc123",
            FileSize = 4096,
            Files = files
        };

        // Act & Assert
        Assert.Equal("abc123", group.Hash);
        Assert.Equal(4096L, group.FileSize);
        Assert.Same(files, group.Files);
        Assert.Equal(2, group.Files.Count);
        Assert.Contains("one.txt", group.Files);
        Assert.Contains("two.txt", group.Files);
    }
}
