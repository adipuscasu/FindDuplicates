using FindDuplicates.Services;
using FindDuplicates.Tests.Helpers;

namespace FindDuplicates.Tests.Services;

public class DuplicateFinderWithFilterTests
{
    [Fact]
    public void GetDuplicateGroups_WithNoFilter_ReturnsAllDuplicates()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var txt1 = Path.Combine(tempRoot, "a1.txt");
            var txt2 = Path.Combine(tempRoot, "a2.txt");
            var jpg1 = Path.Combine(tempRoot, "b1.jpg");
            var jpg2 = Path.Combine(tempRoot, "b2.jpg");

            File.WriteAllText(txt1, "duplicate content");
            File.WriteAllText(txt2, "duplicate content");
            File.WriteAllText(jpg1, "duplicate content");
            File.WriteAllText(jpg2, "duplicate content");

            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());

            // Act
            var groups = finder.GetDuplicateGroups(tempRoot);

            // Assert
            Assert.NotNull(groups);
            // All 4 files share the same content, so there should be one group of 4
            Assert.Single(groups);
            var group = groups.Values.First();
            Assert.Equal(4, group.Files.Count);
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }

    [Fact]
    public void GetDuplicateGroups_WithJpgFilter_ReturnsOnlyJpgDuplicates()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var txt1 = Path.Combine(tempRoot, "a1.txt");
            var txt2 = Path.Combine(tempRoot, "a2.txt");
            var jpg1 = Path.Combine(tempRoot, "b1.jpg");
            var jpg2 = Path.Combine(tempRoot, "b2.jpg");

            File.WriteAllText(txt1, "duplicate content");
            File.WriteAllText(txt2, "duplicate content");
            File.WriteAllText(jpg1, "duplicate content");
            File.WriteAllText(jpg2, "duplicate content");

            var filter = ExtensionFileFilter.FromCommaSeparated("jpg");
            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());

            // Act
            var groups = finder.GetDuplicateGroups(tempRoot, filter);

            // Assert
            Assert.NotNull(groups);
            // Only jpg files should be in the result
            Assert.Single(groups);
            var group = groups.Values.First();
            Assert.Equal(2, group.Files.Count);
            Assert.All(group.Files, f => Assert.EndsWith(".jpg", f, StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }

    [Fact]
    public void GetDuplicateGroups_WithMultipleFilters_ReturnsMatchingDuplicates()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var jpg1 = Path.Combine(tempRoot, "a1.jpg");
            var jpg2 = Path.Combine(tempRoot, "a2.jpg");
            var png1 = Path.Combine(tempRoot, "b1.png");
            var png2 = Path.Combine(tempRoot, "b2.png");
            var txt1 = Path.Combine(tempRoot, "c1.txt");
            var txt2 = Path.Combine(tempRoot, "c2.txt");

            File.WriteAllText(jpg1, "jpg content");
            File.WriteAllText(jpg2, "jpg content");
            File.WriteAllText(png1, "png content");
            File.WriteAllText(png2, "png content");
            File.WriteAllText(txt1, "txt content");
            File.WriteAllText(txt2, "txt content");

            var filter = ExtensionFileFilter.FromCommaSeparated("jpg,png");
            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());

            // Act
            var groups = finder.GetDuplicateGroups(tempRoot, filter);

            // Assert
            Assert.NotNull(groups);
            Assert.Equal(2, groups.Count);
            // Each group should have 2 files
            foreach (var group in groups.Values)
            {
                Assert.Equal(2, group.Files.Count);
            }
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }

    [Fact]
    public void GetDuplicateGroups_WithNullFilter_ReturnsAllDuplicates()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var txt1 = Path.Combine(tempRoot, "a1.txt");
            var txt2 = Path.Combine(tempRoot, "a2.txt");

            File.WriteAllText(txt1, "duplicate content");
            File.WriteAllText(txt2, "duplicate content");

            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());

            // Act
            var groups = finder.GetDuplicateGroups(tempRoot, fileFilter: null);

            // Assert
            Assert.NotNull(groups);
            Assert.Single(groups);
            Assert.Equal(2, groups.Values.First().Files.Count);
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }

    [Fact]
    public void GetDuplicateGroups_WithCaseInsensitiveFilter_MatchesAllCaseVariants()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var jpgUpper = Path.Combine(tempRoot, "a1.JPG");
            var jpgMixed = Path.Combine(tempRoot, "a2.Jpg");
            var jpgLower = Path.Combine(tempRoot, "a3.jpg");

            File.WriteAllText(jpgUpper, "jpg content");
            File.WriteAllText(jpgMixed, "jpg content");
            File.WriteAllText(jpgLower, "jpg content");

            var filter = ExtensionFileFilter.FromCommaSeparated("JPG");
            var finder = new DuplicateFinder(new FakeLogger<DuplicateFinder>());

            // Act
            var groups = finder.GetDuplicateGroups(tempRoot, filter);

            // Assert
            Assert.NotNull(groups);
            Assert.Single(groups);
            Assert.Equal(3, groups.Values.First().Files.Count);
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }
}
