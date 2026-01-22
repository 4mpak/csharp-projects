namespace Task3.Service;

public interface IConsoleService
{
    Task Run(CancellationToken token);
}