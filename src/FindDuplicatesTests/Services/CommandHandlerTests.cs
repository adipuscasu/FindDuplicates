using System;
using System.IO;
using System.Linq;
using Xunit;
using FindDuplicates.Services;
using Microsoft.Extensions.Logging;
using FindDuplicates.Tests.Helpers;

namespace FindDuplicates.Tests.Services;

public class CommandHandlerTests
{
    // Fake finder used to observe Execute calls originating from the processors.
    private class FakeFinder : IDuplicateFinder
    {
        public string? ReceivedPath { get; private set; }
        public System.Collections.Generic.Dictionary<string, FindDuplicates.Models.DuplicateGroup> GetDuplicateGroups(string rootPath)
        {
            ReceivedPath = rootPath;
            return new System.Collections.Generic.Dictionary<string, FindDuplicates.Models.DuplicateGroup>();
        }
    }

    private class FakeUsageDisplay : IUsageDisplay
    {
        public bool ShowUsageCalled { get; private set; }
        public void ShowUsage() => ShowUsageCalled = true;
    }



    [Fact]
    public void HandleCommand_NoArgs_ShowsUsage()
    {
        // Arrange
        var displayFinder = new FakeFinder();
        var removeFinder = new FakeFinder();
        var display = new DuplicateDisplayer(displayFinder, new FakeLogger<DuplicateDisplayer>());
        var remove = new DuplicateRemover(removeFinder, new FakeLogger<DuplicateRemover>());
        var usage = new FakeUsageDisplay();
        var handler = new CommandHandler(display, remove, usage, new FakeLogger<CommandHandler>());

        // Act
        handler.HandleCommand(Array.Empty<string>());

        // Assert
        Assert.True(usage.ShowUsageCalled);
    }

    [Fact]
    public void HandleCommand_UnknownCommand_ShowsUsageAndMessage()
    {
        // Arrange
        var displayFinder = new FakeFinder();
        var removeFinder = new FakeFinder();
        var display = new DuplicateDisplayer(displayFinder, new FakeLogger<DuplicateDisplayer>());
        var remove = new DuplicateRemover(removeFinder, new FakeLogger<DuplicateRemover>());
        var usage = new FakeUsageDisplay();
        var handler = new CommandHandler(display, remove, usage, new FakeLogger<CommandHandler>());

        using var sw = new StringWriter();
        var prevOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            // Act
            handler.HandleCommand(new[] { "unknown", Directory.GetCurrentDirectory() });

            // Assert
            var output = sw.ToString();
            Assert.Contains("Unknown command", output, StringComparison.OrdinalIgnoreCase);
            Assert.True(usage.ShowUsageCalled);
        }
        finally
        {
            Console.SetOut(prevOut);
        }
    }

    [Fact]
    public void HandleCommand_NonExistentFolder_PrintsError()
    {
        // Arrange
        var displayFinder = new FakeFinder();
        var removeFinder = new FakeFinder();
        var display = new DuplicateDisplayer(displayFinder, new FakeLogger<DuplicateDisplayer>());
        var remove = new DuplicateRemover(removeFinder, new FakeLogger<DuplicateRemover>());
        var usage = new FakeUsageDisplay();
        var handler = new CommandHandler(display, remove, usage, new FakeLogger<CommandHandler>());

        using var sw = new StringWriter();
        var prevOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            // Act
            handler.HandleCommand(new[] { "find", "Z:\\this\\folder\\should\\not\\exist\\_12345" });

            // Assert
            var output = sw.ToString();
            Assert.Contains("does not exist", output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            Console.SetOut(prevOut);
        }
    }

    [Fact]
    public void HandleCommand_FindWithoutRemove_CallsDisplayProcessor()
    {
        // Arrange
        var displayFinder = new FakeFinder();
        var removeFinder = new FakeFinder();
        var display = new DuplicateDisplayer(displayFinder, new FakeLogger<DuplicateDisplayer>());
        var remove = new DuplicateRemover(removeFinder, new FakeLogger<DuplicateRemover>());
        var usage = new FakeUsageDisplay();
        var handler = new CommandHandler(display, remove, usage, new FakeLogger<CommandHandler>());

        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            // Act
            handler.HandleCommand(new[] { "find", tempRoot });

            // Assert: display's finder should have been called with the tempRoot
            Assert.Equal(tempRoot, displayFinder.ReceivedPath);
            Assert.Null(removeFinder.ReceivedPath);
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }

    [Fact]
    public void HandleCommand_FindWithRemoveFlag_CallsRemoveProcessor()
    {
        // Arrange
        var displayFinder = new FakeFinder();
        var removeFinder = new FakeFinder();
        var display = new DuplicateDisplayer(displayFinder, new FakeLogger<DuplicateDisplayer>());
        var remove = new DuplicateRemover(removeFinder, new FakeLogger<DuplicateRemover>());
        var usage = new FakeUsageDisplay();
        var handler = new CommandHandler(display, remove, usage, new FakeLogger<CommandHandler>());

        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            // Act (flag before folder)
            handler.HandleCommand(new[] { "find", "--remove", tempRoot });

            // Assert
            Assert.Equal(tempRoot, removeFinder.ReceivedPath);
            Assert.Null(displayFinder.ReceivedPath);
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }

    [Fact]
    public void HandleCommand_RemoveCommand_CallsRemoveProcessor()
    {
        // Arrange
        var displayFinder = new FakeFinder();
        var removeFinder = new FakeFinder();
        var display = new DuplicateDisplayer(displayFinder, new FakeLogger<DuplicateDisplayer>());
        var remove = new DuplicateRemover(removeFinder, new FakeLogger<DuplicateRemover>());
        var usage = new FakeUsageDisplay();
        var handler = new CommandHandler(display, remove, usage, new FakeLogger<CommandHandler>());

        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            // Act
            handler.HandleCommand(new[] { "remove", tempRoot });

            // Assert
            Assert.Equal(tempRoot, removeFinder.ReceivedPath);
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }
}