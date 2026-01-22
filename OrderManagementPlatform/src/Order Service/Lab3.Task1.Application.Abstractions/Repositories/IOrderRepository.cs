using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;
using Lab3.Task1.Domain.Enums;

namespace Lab3.Task1.Application.Abstractions.Repositories;

public interface IOrderRepository
{
    Task<long> CreateOrderAsync(Order order);

    Task<int> ChangeOrderStateAsync(long orderId, OrderState newState);

    IAsyncEnumerable<Order> QueryOrderAsync(OrderQuery query);
}