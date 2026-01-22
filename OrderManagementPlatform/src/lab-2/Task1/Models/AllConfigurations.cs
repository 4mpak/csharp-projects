namespace Task1.Models;

public class AllConfigurations
{
    public IEnumerable<ConfigurationItemDto> ConfigurationItems { get; set; } = new List<ConfigurationItemDto>();

    public void AddConfigurationItem(IEnumerable<ConfigurationItemDto> dto)
    {
        ConfigurationItems.Concat(dto);
    }

    public IEnumerable<ConfigurationItemDto> ReadConfigurationItems()
    {
        return ConfigurationItems;
    }
}