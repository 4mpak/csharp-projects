using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;

namespace Lab3.Task1.Application.Abstractions.Repositories;

public interface IOrderHistoryRepository
{
    Task<long> CreateOrderHistoryAsync(OrderHistory orderHistory);

    IAsyncEnumerable<OrderHistory> QueryOrderHistoryAsync(OrderHistoryQuery query);
}