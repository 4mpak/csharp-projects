using Microsoft.Extensions.Configuration;
using Task1.Models;

namespace Task2.Provider;

public class MyConfigurationsProvider : ConfigurationProvider
{
    public void OnConfigurationUpdated(AllConfigurations configuration)
    {
        if (!configuration.ConfigurationItems.Any())
        {
            Data.Clear();
            OnReload();
            return;
        }

        var updateData = configuration.ReadConfigurationItems().ToDictionary(dto => dto.Key, dto => dto.Value.ToString());

        if (Data.Count != updateData.Count)
        {
            Data.Clear();
            foreach (ConfigurationItemDto dto in configuration.ReadConfigurationItems())
            {
                Data[$"{dto.Key}"] = dto.Value.ToString();
            }

            OnReload();
            return;
        }

        foreach (ConfigurationItemDto dto in configuration.ReadConfigurationItems())
        {
            Data[$"{dto.Key}"] = dto.Value.ToString();
        }
    }
}