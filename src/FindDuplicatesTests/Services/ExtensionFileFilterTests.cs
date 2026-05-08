using FindDuplicates.Services;

namespace FindDuplicates.Tests.Services;

public class ExtensionFileFilterTests
{
    #region FromCommaSeparated

    [Fact]
    public void FromCommaSeparated_SingleExtension_CreatesFilter()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromCommaSeparated("jpg");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("photo.jpg"));
        Assert.True(filter.Matches("photo.JPG"));
        Assert.False(filter.Matches("photo.png"));
    }

    [Fact]
    public void FromCommaSeparated_MultipleExtensions_CreatesFilter()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromCommaSeparated("jpg,png,gif");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("photo.jpg"));
        Assert.True(filter.Matches("image.PNG"));
        Assert.True(filter.Matches("animation.gif"));
        Assert.False(filter.Matches("document.pdf"));
    }

    [Fact]
    public void FromCommaSeparated_WithLeadingDots_Normalizes()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromCommaSeparated(".jpg,.png");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("photo.jpg"));
        Assert.True(filter.Matches("image.png"));
    }

    [Fact]
    public void FromCommaSeparated_WithSpaces_Trimmed()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromCommaSeparated(" jpg ,  png , gif ");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("photo.jpg"));
        Assert.True(filter.Matches("image.PNG"));
        Assert.True(filter.Matches("anim.gif"));
    }

    [Fact]
    public void FromCommaSeparated_EmptyString_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => ExtensionFileFilter.FromCommaSeparated(""));
    }

    [Fact]
    public void FromCommaSeparated_OnlySpaces_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => ExtensionFileFilter.FromCommaSeparated("   "));
    }

    [Fact]
    public void FromCommaSeparated_AllEmptySegments_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => ExtensionFileFilter.FromCommaSeparated(",,"));
    }

    [Fact]
    public void FromCommaSeparated_MixedEmptySegments_CreatesFilter()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromCommaSeparated("jpg,,png,");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("photo.jpg"));
        Assert.True(filter.Matches("image.png"));
    }

    #endregion

    #region FromArray

    [Fact]
    public void FromArray_SingleExtension_CreatesFilter()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromArray("docx");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("file.docx"));
        Assert.False(filter.Matches("file.pdf"));
    }

    [Fact]
    public void FromArray_MultipleExtensions_CreatesFilter()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromArray("txt", "pdf", "doc");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("file.txt"));
        Assert.True(filter.Matches("file.PDF"));
        Assert.True(filter.Matches("file.doc"));
        Assert.False(filter.Matches("file.xls"));
    }

    [Fact]
    public void FromArray_EmptyArray_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => ExtensionFileFilter.FromArray());
    }

    [Fact]
    public void FromArray_NullArray_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => ExtensionFileFilter.FromArray(null!));
    }

    [Fact]
    public void FromArray_WithDots_Normalizes()
    {
        // Arrange & Act
        var filter = ExtensionFileFilter.FromArray(".jpg", ".PNG");

        // Assert
        Assert.NotNull(filter);
        Assert.True(filter.Matches("photo.JPG"));
        Assert.True(filter.Matches("image.png"));
    }

    #endregion

    #region Matches

    [Fact]
    public void Matches_PathsWithDifferentCasing_AllMatch()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromArray("jpg");

        // Act & Assert
        Assert.True(filter.Matches("photo.JPG"));
        Assert.True(filter.Matches("photo.Jpg"));
        Assert.True(filter.Matches("photo.jPg"));
        Assert.True(filter.Matches("photo.jpg"));
    }

    [Fact]
    public void Matches_PathsInSubdirectories_MatchesExtension()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromCommaSeparated("mp3,wav");

        // Act & Assert
        Assert.True(filter.Matches("/home/user/music/album/song.mp3"));
        Assert.True(filter.Matches("C:\\Music\\song.WAV"));
        Assert.False(filter.Matches("C:\\Music\\cover.jpg"));
    }

    [Fact]
    public void Matches_QueryStringInPath_UsesLastDotSegment()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromArray("jpg");

        // Act & Assert
        // Path.GetExtension returns everything after the LAST dot, so .jpg?v=1 yields .v=1
        // The filter only knows extensions, so it will NOT match .v=1
        Assert.False(filter.Matches("photo.jpg?v=1"));
    }

    [Fact]
    public void Matches_FilenameWithMultipleDots_UsesLastExtension()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromArray("txt");

        // Act & Assert
        Assert.True(filter.Matches("file.backup.txt"));
        Assert.False(filter.Matches("file.backup.bak"));
    }

    [Fact]
    public void Matches_NullPath_ReturnsFalse()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromArray("jpg");

        // Act & Assert
        Assert.False(filter.Matches(null!));
    }

    [Fact]
    public void Matches_EmptyPath_ReturnsFalse()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromArray("jpg");

        // Act & Assert
        Assert.False(filter.Matches(""));
    }

    [Fact]
    public void Matches_WhitespacePath_ReturnsFalse()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromArray("jpg");

        // Act & Assert
        Assert.False(filter.Matches("   "));
    }

    [Fact]
    public void Matches_NoExtension_ReturnsFalse()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromArray("jpg");

        // Act & Assert
        Assert.False(filter.Matches("README"));
        Assert.False(filter.Matches("/usr/bin/git"));
    }

    [Fact]
    public void Matches_DuplicateExtensionsInList_AllMatch()
    {
        // Arrange
        var filter = ExtensionFileFilter.FromCommaSeparated("jpg,JPG,Jpg");

        // Act & Assert
        Assert.True(filter.Matches("photo.jpg"));
        Assert.True(filter.Matches("photo.JPG"));
    }

    #endregion
}
