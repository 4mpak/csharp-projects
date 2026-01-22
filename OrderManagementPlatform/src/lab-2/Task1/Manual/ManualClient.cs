using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Task1.Interfaces;
using Task1.Models;
using Task1.Options;

namespace Task1.Manual;

public class ManualClient : IUploadingСonfigurationClient
{
    private readonly JsonSerializerOptions options;
    private readonly HttpClient client;
    private readonly SettingsOptions settings;

    public ManualClient(HttpClient httpClient, IOptionsMonitor<SettingsOptions> settingsOptions)
    {
        client = httpClient;
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        settings = settingsOptions.CurrentValue;
    }

    public async IAsyncEnumerable<ConfigurationItemDto> GetConfigurations([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        QueryConfigurationsResponse page;
        string? pageToken = null;
        do
        {
            page = await SendRequest(settings.PageSize, pageToken);
            foreach (ConfigurationItemDto dto in page.Items)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return dto;
            }

            pageToken = page.PageToken;
        }
        while (pageToken is not null);
    }

    private async Task<QueryConfigurationsResponse> SendRequest(int pageSize, string? pageToken)
    {
        string url = $"configurations?pageSize={pageSize}&pageToken={pageToken}";
        HttpResponseMessage response = await client.GetAsync(url);
        return await JsonSerializer.DeserializeAsync<QueryConfigurationsResponse>(await response.Content.ReadAsStreamAsync(), options) ?? throw new JsonException();
    }
}