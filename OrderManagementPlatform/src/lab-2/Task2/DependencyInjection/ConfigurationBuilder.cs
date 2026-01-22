using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task2.Provider;
using Task2.Source;

namespace Task2.DependencyInjection;

public static class ConfigurationBuilder
{
    public static IServiceCollection SettingBuilder(this IServiceCollection serviceCollection, IConfigurationBuilder builder)
    {
        var provider = new MyConfigurationsProvider();
        builder.Add(new ConfigurationsSource(provider));
        serviceCollection.AddSingleton(provider);
        return serviceCollection;
    }
}