
using Microsoft.Extensions.Logging;

namespace FindDuplicates.Tests.Helpers;



// Lightweight generic fake logger that writes messages to Console so existing tests
// that capture Console.Out continue to work.

internal sealed class FakeLogger<T> : ILogger<T>
{
    IDisposable? ILogger.BeginScope<TState>(TState state) where TState: default
        => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (formatter is null) return;
        var message = formatter(state, exception);
        // Write simple output that tests expect (contains key phrases).
        Console.WriteLine(message);
        if (exception is not null)
        {
            Console.WriteLine(exception);
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}
