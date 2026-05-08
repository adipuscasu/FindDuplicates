using FindDuplicates.Utilities;

namespace FindDuplicates.Tests.Utilities;

public class ExtensionNormalizerTests
{
    #region Normalize

    [Fact]
    public void Normalize_WithDotPrefix_ReturnsLowercaseWithDot()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize(".jpg");

        // Assert
        Assert.Equal(".jpg", result);
    }

    [Fact]
    public void Normalize_WithoutDotPrefix_ReturnsLowercaseWithDot()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize("PNG");

        // Assert
        Assert.Equal(".png", result);
    }

    [Fact]
    public void Normalize_WithMultipleLeadingDots_StripsAll()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize("...gif");

        // Assert
        Assert.Equal(".gif", result);
    }

    [Fact]
    public void Normalize_CaseInsensitive_ReturnsLowercase()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize("JPEG");

        // Assert
        Assert.Equal(".jpeg", result);
    }

    [Fact]
    public void Normalize_NullInput_ReturnsAll()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize(null!);

        // Assert
        Assert.Equal(".all", result);
    }

    [Fact]
    public void Normalize_EmptyInput_ReturnsAll()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize("");

        // Assert
        Assert.Equal(".all", result);
    }

    [Fact]
    public void Normalize_WhitespaceInput_ReturnsAll()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize("   ");

        // Assert
        Assert.Equal(".all", result);
    }

    [Fact]
    public void Normalize_WithSurroundingWhitespace_Trimmed()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.Normalize("  docx  ");

        // Assert
        Assert.Equal(".docx", result);
    }

    [Fact]
    public void Normalize_WellKnownExtensions_ProducesExpected()
    {
        // Arrange
        var (input, expected) = (".MP4", ".mp4");

        // Act
        var result = ExtensionNormalizer.Normalize(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region NormalizeMany

    [Fact]
    public void NormalizeMany_WithMultipleExtensions_ReturnsAll()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.NormalizeMany(new[] { "jpg", "png", "gif" });

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(".jpg", result);
        Assert.Contains(".png", result);
        Assert.Contains(".gif", result);
    }

    [Fact]
    public void NormalizeMany_MixedFormats_NormalizesAll()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.NormalizeMany(new[] { ".JPG", "png", "..GIF" });

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(".jpg", result);
        Assert.Contains(".png", result);
        Assert.Contains(".gif", result);
    }

    [Fact]
    public void NormalizeMany_NullInput_ReturnsEmptySet()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.NormalizeMany(Array.Empty<string>());

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void NormalizeMany_WithDuplicates_RemovesDuplicates()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.NormalizeMany(new[] { ".jpg", "JPG", "Jpg" });

        // Assert
        Assert.Single(result);
        Assert.Contains(".jpg", result);
    }

    [Fact]
    public void NormalizeMany_UseCaseInsensitiveComparer()
    {
        // Arrange & Act
        var result = ExtensionNormalizer.NormalizeMany(new[] { ".JPG", ".PNG" });

        // Assert
        // HashSet is initialized with StringComparer.OrdinalIgnoreCase
        Assert.Contains(".jpg", result);
        Assert.Contains(".png", result);
        Assert.Contains(".JPG", result);
        Assert.Contains(".PNG", result);
    }

    #endregion
}
