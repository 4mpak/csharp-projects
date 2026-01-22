using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;

namespace Lab3.Task1.Application.Abstractions.Repositories;

public interface IOrderItemRepository
{
    Task<long> CreateOrderItemAsync(OrderItem orderItem);

    Task<long> DeleteOrderItemAsync(long orderItemId);

    Task<int> DeleteWithProductIdAsync(long orderId, long productId);

    IAsyncEnumerable<OrderItem> QueryOrderItemAsync(OrderItemQuery query);
}