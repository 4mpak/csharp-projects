using Microsoft.Extensions.Configuration;
using Task2.Provider;

namespace Task2.Source;

public class ConfigurationsSource : IConfigurationSource
{
    private readonly MyConfigurationsProvider _provider;

    public ConfigurationsSource(MyConfigurationsProvider provider)
    {
        _provider = provider;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return _provider;
    }
}