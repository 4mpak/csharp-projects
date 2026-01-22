using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using Task1.Interfaces;
using Task1.Options;

namespace Task1.DependencyInjection;

public static class RefitClientExtensions
{
    public static IServiceCollection AddRefitClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRefitClient<IRefitConfigurationsClient>().ConfigureHttpClient((provider, client) =>
        {
            string link = provider.GetRequiredService<IOptions<SettingsOptions>>().Value.Link;
            client.BaseAddress = new Uri(link);
        });
        return serviceCollection;
    }
}