using Task1.Models;

namespace Task1.Interfaces;

public interface IUploadingСonfigurationClient
{
    IAsyncEnumerable<ConfigurationItemDto> GetConfigurations(CancellationToken cancellationToken);
}