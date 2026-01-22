namespace Task1.Models;

public class ConfigurationItemDto
{
    public string Key { get; set; }

    public string Value { get; set; }

    public ConfigurationItemDto(string key, string value)
    {
        Key = key;
        Value = value;
    }
}