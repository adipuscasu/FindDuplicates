using System;
using System.IO;
using Xunit;
using FindDuplicates.Services;
using Microsoft.Extensions.Logging;

namespace FindDuplicates.Tests;

public class DuplicateRemoverDeterministicTests
{
    // Lightweight fake logger that writes to Console for test visibility.
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
