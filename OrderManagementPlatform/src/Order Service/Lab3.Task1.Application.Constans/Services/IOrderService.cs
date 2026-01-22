using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task1.Domain.Enums;

namespace Lab3.Task1.Application.Constans.Services;

public interface IOrderService
{
    Task<long> CreateOrderAsync(OrderCreatingDto dto);

    Task<long> AddProductAsync(long orderId, long productId, int quantity);

    Task<int> DeleteProductAsync(long orderId, long productId);

    Task<int> ChangeStateToProcessingAsync(long orderId);

    Task<int> ChangeStateToCompletedAsync(long orderId);

    Task<int> ChangeStateToCancelledAsync(long orderId);

    IAsyncEnumerable<OrderHistoryDto> QueryOrderHistory(long orderId, OrderHistoryItemKind? kind, int pageSize, int cursor);

    Task AddOrderProcessingHistory(long orderId, HistoryCreatingDto dto);
}