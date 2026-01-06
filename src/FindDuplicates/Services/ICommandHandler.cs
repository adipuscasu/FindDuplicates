namespace FindDuplicates.Services;

public interface ICommandHandler
{
    void HandleCommand(string[] args);
}