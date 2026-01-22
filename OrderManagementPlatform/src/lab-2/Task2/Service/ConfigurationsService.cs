using Microsoft.Extensions.Options;
using Task1.Interfaces;
using Task1.Models;
using Task2.Interfaces;
using Task2.Options;
using Task2.Provider;

namespace Task2.Service;

public class ConfigurationsService : IConfigurationsService
{
    private readonly IOptions<ServiceOptions> options;
    private readonly IUploadingСonfigurationClient client;
    private readonly MyConfigurationsProvider provider;

    public ConfigurationsService(MyConfigurationsProvider myProvider, IUploadingСonfigurationClient сonfigurationClient, IOptions<ServiceOptions> serviceOptions)
    {
        client = сonfigurationClient;
        provider = myProvider;
        options = serviceOptions;
    }

    public async Task FetchAndUpdateConfigurations(CancellationToken cancellationToken)
    {
        var configList = new List<ConfigurationItemDto>();
        await foreach (ConfigurationItemDto item in client.GetConfigurations(cancellationToken))
        {
            configList.Add(item);
        }

        var configurations = new AllConfigurations()
        {
            ConfigurationItems = configList,
        };
        provider.OnConfigurationUpdated(configurations);
    }

    public async Task Run(CancellationToken token)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(options.Value.Seconds));
        while (await timer.WaitForNextTickAsync(token))
        {
            await FetchAndUpdateConfigurations(token);
        }
    }
}