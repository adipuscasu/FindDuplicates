using FindDuplicates.Services;

namespace FindDuplicates.Application;

public class App
{
    private readonly ICommandHandler _commandHandler;

    public App(ICommandHandler commandHandler)
    {
        _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
    }

    public void Run(string[] args)
    {
        _commandHandler.HandleCommand(args);
    }
}