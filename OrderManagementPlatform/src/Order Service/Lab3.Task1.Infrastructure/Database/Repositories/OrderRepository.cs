using Lab3.Task1.Application.Abstractions.Repositories;
using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;
using Lab3.Task1.Domain.Enums;
using Lab3.Task1.Infrastructure.Database.Connections;
using Npgsql;
using System.Data;

namespace Lab3.Task1.Infrastructure.Database.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PostgresProvider _postgresProvider;

    public OrderRepository(PostgresProvider postgresProvider)
    {
        _postgresProvider = postgresProvider;
    }

    public async Task<long> CreateOrderAsync(Order order)
    {
        const string sql = """
                           insert into orders (order_state, order_created_at, order_created_by)
                           values (:order_state, :order_created_at, :order_created_by)
                           returning order_id;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_state", order.OrderState),
                new NpgsqlParameter("order_created_at", order.OrderCreatedAt),
                new NpgsqlParameter("order_created_by", order.OrderCreatedBy),
            },
        };

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt64("order_id");
        }

        throw new InvalidOperationException();
    }

    public async Task<int> ChangeOrderStateAsync(long orderId, OrderState newState)
    {
        const string sql = """
                           update orders
                           set order_state = :order_state
                           where order_id = :order_id
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_state", newState),
                new NpgsqlParameter("order_id", orderId),
            },
        };

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        return reader.RecordsAffected;
    }

    public async IAsyncEnumerable<Order> QueryOrderAsync(OrderQuery query)
    {
        const string sql = """
                           select order_id, order_state, order_created_at, order_created_by
                           from orders
                           where
                               (order_id > :cursor)
                               and (cardinality(:ids) = 0 or order_id = any (:ids))
                               and (:order_state::order_state is null or order_state = :order_state)
                               and (:author::text is null or order_created = :author)
                           limit :page_size;
                           """;

        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("ids", query.Ids),
                new NpgsqlParameter("order_state", query.State),
                new NpgsqlParameter("author", query.OrderCreatedBy),
                new NpgsqlParameter("cursor", query.Cursor),
                new NpgsqlParameter("page_size", query.PageSize),
            },
        };

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return new Order
            {
                OrderId = DataReaderExtensions.GetInt64(reader, "order_id"),
                OrderState = DataReaderExtensions.GetFieldValue<OrderState>(reader, "order_state"),
                OrderCreatedAt = DataReaderExtensions.GetFieldValue<DateTimeOffset>(reader, "order_created_at"),
                OrderCreatedBy = DataReaderExtensions.GetString(reader, "order_created_by"),
            };
        }
    }
}