using Refit;
using Task1.Models;

namespace Task1.Interfaces;

public interface IRefitConfigurationsClient
{
    [Get("configurations")]
    Task<QueryConfigurationsResponse> GetConfigurations(int pageSize, string? pageToken);
}