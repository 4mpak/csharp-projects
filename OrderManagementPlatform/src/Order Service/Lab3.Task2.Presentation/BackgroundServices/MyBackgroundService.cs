using Task2.Interfaces;

namespace Lab3.Task2.Presentation.BackgroundServices;

public class MyBackgroundService : BackgroundService
{
    private readonly IConfigurationsService _configurationService;

    public MyBackgroundService(IConfigurationsService configurationService)
    {
        _configurationService = configurationService;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _configurationService.FetchAndUpdateConfigurations(cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _configurationService.FetchAndUpdateConfigurations(stoppingToken);
    }
}