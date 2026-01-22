namespace Task1.Models;

public class QueryConfigurationsResponse
{
    public IEnumerable<ConfigurationItemDto> Items { get; set; } = [];

    public string? PageToken { get; set; }
}