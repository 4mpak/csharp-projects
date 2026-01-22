using Lab3.Task1.Application.Abstractions.Repositories;
using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;
using Lab3.Task1.Infrastructure.Database.Connections;
using Npgsql;
using System.Data;

namespace Lab3.Task1.Infrastructure.Database.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly PostgresProvider _postgresProvider;

    public OrderItemRepository(PostgresProvider postgresProvider)
    {
        _postgresProvider = postgresProvider;
    }

    public async Task<long> CreateOrderItemAsync(OrderItem orderItem)
    {
        const string sql = """
                           insert into order_items (order_id, product_id, order_item_quantity, order_item_deleted)
                           values (:order_id, :product_id, :order_item_quantity, :order_item_deleted)
                           returning order_item_id;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_id", orderItem.OrderId),
                new NpgsqlParameter("product_id", orderItem.ProductId),
                new NpgsqlParameter("order_item_quantity", orderItem.OrderItemQuantity),
                new NpgsqlParameter("order_item_deleted", orderItem.OrderItemDeleted),
            },
        };

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt64("order_item_id");
        }

        throw new InvalidOperationException();
    }

    public async Task<long> DeleteOrderItemAsync(long orderItemId)
    {
        const string sql = """
                           update order_items
                           set order_item_deleted = true
                           where  order_item_id = :order_item_id
                           returning order_item_id;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_item_id", orderItemId),
            },
        };

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt64("order_item_id");
        }

        throw new InvalidOperationException();
    }

    public async Task<int> DeleteWithProductIdAsync(long orderId, long productId)
    {
        const string sql = """
                           update order_items
                           set order_item_deleted = true
                           where order_id = :order_id
                           and product_id = :product_id
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_id", orderId),
                new NpgsqlParameter("product_id", productId),
            },
        };

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        return reader.RecordsAffected;
    }

    public async IAsyncEnumerable<OrderItem> QueryOrderItemAsync(OrderItemQuery query)
    {
        const string sql = """
                           select order_item_id, order_id, product_id, order_item_quantity, order_item_deleted
                           from order_items
                           where
                               (order_item_id > :cursor)
                               and (cardinality(:order_ids) = 0 or order_id = any (:order_ids))
                               and (cardinality(:product_ids) = 0 or product_id = any (:product_ids))
                               and (:deleted is null or order_item_deleted = :deleted)
                           limit :page_size;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_ids", query.OrderIds),
                new NpgsqlParameter("product_ids", query.ProductIds),
                new NpgsqlParameter("deleted", query.Deleted),
                new NpgsqlParameter("cursor", query.Cursor),
                new NpgsqlParameter("page_size", query.PageSize),
            },
        };

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return new OrderItem
            {
                OrderItemId = DataReaderExtensions.GetInt64(reader, "order_item_id"),
                OrderId = DataReaderExtensions.GetInt64(reader, "order_id"),
                ProductId = DataReaderExtensions.GetInt64(reader, "product_id"),
                OrderItemQuantity = DataReaderExtensions.GetInt32(reader, "order_item_quantity"),
                OrderItemDeleted = DataReaderExtensions.GetBoolean(reader, "order_item_deleted"),
            };
        }
    }
}