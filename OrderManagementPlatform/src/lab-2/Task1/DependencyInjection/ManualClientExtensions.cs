using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Task1.Interfaces;
using Task1.Manual;
using Task1.Options;

namespace Task1.DependencyInjection;

public static class ManualClientExtensions
{
    public static IServiceCollection AddManualClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<IUploadingСonfigurationClient, ManualClient>((provider, client) =>
        {
            string link = provider.GetRequiredService<IOptions<SettingsOptions>>().Value.Link;
            client.BaseAddress = new Uri(link);
        });

        return serviceCollection;
    }
}