using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using Task1.Interfaces;
using Task1.Models;
using Task1.Options;

namespace Task1.Refit;

public class RefitClient : IUploadingСonfigurationClient
{
    private readonly IRefitConfigurationsClient refitConfigurationsClient;
    private readonly SettingsOptions settings;

    public RefitClient(IRefitConfigurationsClient refitClient, IOptionsMonitor<SettingsOptions> settingsOptions)
    {
        refitConfigurationsClient = refitClient;
        settings = settingsOptions.CurrentValue;
    }

    public async IAsyncEnumerable<ConfigurationItemDto> GetConfigurations([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        QueryConfigurationsResponse page;
        string? pageToken = null;
        do
        {
            page = await refitConfigurationsClient.GetConfigurations(settings.PageSize, pageToken);
            foreach (ConfigurationItemDto dto in page.Items)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return dto;
            }

            pageToken = page.PageToken;
        }
        while (pageToken is not null);
    }
}