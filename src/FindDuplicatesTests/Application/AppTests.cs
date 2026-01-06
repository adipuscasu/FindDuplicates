using System;
using Xunit;
using FindDuplicates.Application;
using FindDuplicates.Services;

namespace FindDuplicates.Tests.Application;

public class AppTests
{
    private class FakeCommandHandler : ICommandHandler
    {
        public string[]? ReceivedArgs { get; private set; }

        public void HandleCommand(string[] args)
        {
            ReceivedArgs = args;
        }
    }

    [Fact]
    public void Run_ForwardsArgsToCommandHandler()
    {
        // Arrange
        var fakeHandler = new FakeCommandHandler();
        var app = new App(fakeHandler);
        var expectedArgs = new[] { "show", "C:\\tmp" };

        // Act
        app.Run(expectedArgs);

        // Assert
        Assert.NotNull(fakeHandler.ReceivedArgs);
        Assert.Equal(expectedArgs, fakeHandler.ReceivedArgs);
    }
}