using System;
using System.IO;
using System.Linq;
using Xunit;
using FindDuplicates.Services;
using Microsoft.Extensions.Logging;

namespace FindDuplicates.Tests.Services;

public class DuplicateFinderTests
{
    // Simple fake logger that writes messages to Console for test visibility.
    private class FakeLogger<T> : ILogger<T>
    {
        private class NoopDisposable : IDisposable { public void Dispose() { } }
        public IDisposable BeginScope<TState>(TState state) => new NoopDisposable();
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (formatter is null) return;
            Console.WriteLine(formatter(state, exception));
            if (exception is not null) Console.WriteLine(exception);
        }
    }

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