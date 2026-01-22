namespace Task2.Interfaces;

public interface IConfigurationsService
{
    Task FetchAndUpdateConfigurations(CancellationToken cancellationToken);

    Task Run(CancellationToken token);
}