using Microsoft.Extensions.Primitives;
using Task1.Models;
using Task2.Provider;
using Xunit;

namespace Lab2.Tests;

public class Task2Tests
{
    [Fact]
    public void Test1()
    {
        var provider = new MyConfigurationsProvider();
        IChangeToken token = provider.GetReloadToken();
        var configuration = new AllConfigurations
        {
            ConfigurationItems = new List<ConfigurationItemDto>
                { new ConfigurationItemDto("koko", "jambo") },
        };

        provider.OnConfigurationUpdated(configuration);

        provider.TryGet("koko", out string? jambo);
        Assert.Equal("jambo", jambo);
        Assert.True(token.HasChanged);
    }

    [Fact]
    public void Test2()
    {
        var configuration = new AllConfigurations
        {
            ConfigurationItems = new List<ConfigurationItemDto>
                { new ConfigurationItemDto("koko", "jambo") },
        };
        var provider = new MyConfigurationsProvider();
        provider.OnConfigurationUpdated(configuration);
        provider.TryGet("koko", out string? initData);
        provider.OnConfigurationUpdated(configuration);
        provider.TryGet("koko", out string? finalData);

        Assert.Equal("jambo", initData);
        Assert.Equal("jambo", finalData);
    }

    [Fact]
    public void Test3()
    {
        var initConfiguration = new AllConfigurations
        {
            ConfigurationItems = new List<ConfigurationItemDto>
                { new ConfigurationItemDto("koko", "jambo") },
        };
        var updateConfiguration = new AllConfigurations
        {
            ConfigurationItems = new List<ConfigurationItemDto>
                { new ConfigurationItemDto("koko", "plaki") },
        };
        var provider = new MyConfigurationsProvider();
        provider.OnConfigurationUpdated(initConfiguration);

        provider.OnConfigurationUpdated(updateConfiguration);

        provider.TryGet("koko", out string? lol);
        Assert.Equal("plaki", lol);
    }

    [Fact]
    public void Test4()
    {
        var provider = new MyConfigurationsProvider();
        var initConfiguration = new AllConfigurations
        {
            ConfigurationItems = new List<ConfigurationItemDto>
                { new ConfigurationItemDto("koko", "jambo") },
        };
        provider.OnConfigurationUpdated(initConfiguration);

        Assert.True(provider.TryGet("koko", out string? initData));
        Assert.Equal("jambo", initData);

        var emptyConfiguration = new AllConfigurations
        {
            ConfigurationItems = new List<ConfigurationItemDto>(),
        };
        provider.OnConfigurationUpdated(emptyConfiguration);
        Assert.False(provider.TryGet("koko", out string? finalData));
        Assert.Null(finalData);
    }
}