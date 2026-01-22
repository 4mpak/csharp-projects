using Lab3.Task1.Application.Abstractions.Repositories;
using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;
using Lab3.Task1.Domain.Enums;
using Lab3.Task1.Domain.Records;
using Lab3.Task1.Infrastructure.Database.Connections;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace Lab3.Task1.Infrastructure.Database.Repositories;

public class OrderHistoryRepository : IOrderHistoryRepository
{
    private readonly PostgresProvider _postgresProvider;

    public OrderHistoryRepository(PostgresProvider postgresProvider)
    {
        _postgresProvider = postgresProvider;
    }

    public async Task<long> CreateOrderHistoryAsync(OrderHistory orderHistory)
    {
        const string sql = """
                           insert into order_history (order_id, order_history_item_created_at, order_history_item_kind, order_history_item_payload)
                           values (:order_id, :order_history_item_created_at, :order_history_item_kind, :order_history_item_payload::jsonb)
                           returning order_history_item_id;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();
        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_id", orderHistory.OrderId),
                new NpgsqlParameter("order_history_item_created_at", orderHistory.OrderHistoryItemCreatedAt),
                new NpgsqlParameter("order_history_item_kind", orderHistory.OrderHistoryItemKind),
                new NpgsqlParameter("order_history_item_payload", JsonSerializer.Serialize(orderHistory.OrderHistoryItemPayload, orderHistory.OrderHistoryItemPayload.GetType())),
            },
        };

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt64("order_history_item_id");
        }

        throw new InvalidOperationException();
    }

    public async IAsyncEnumerable<OrderHistory> QueryOrderHistoryAsync(OrderHistoryQuery query)
    {
        const string sql = """
                           select order_history_item_id, order_id, order_history_item_created_at, order_history_item_kind, order_history_item_payload
                           from order_history
                           where
                               (order_history_item_id > :cursor)
                               and (cardinality(:order_ids) = 0 or order_id = any (:order_ids))
                               and (:order_history_item_kind::order_history_item_kind is null or order_history_item_kind = :order_history_item_kind)
                               limit :page_size;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("order_ids", query.OrderIds),
                new NpgsqlParameter("order_history_item_kind", query.HistoryKind is null ? DBNull.Value : query.HistoryKind),
                new NpgsqlParameter("cursor", query.Cursor),
                new NpgsqlParameter("page_size", query.PageSize),
            },
        };

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            HistoryItemPayload payload = JsonSerializer.Deserialize<HistoryItemPayload>(DataReaderExtensions.GetString(reader, "order_history_item_payload")) ?? throw new InvalidOperationException();
            yield return new OrderHistory
            {
                OrderHistoryItemId = DataReaderExtensions.GetInt64(reader, "order_history_item_id"),
                OrderId = DataReaderExtensions.GetInt64(reader, "order_id"),
                OrderHistoryItemCreatedAt = DataReaderExtensions.GetFieldValue<DateTimeOffset>(reader, "order_history_item_created_at"),
                OrderHistoryItemKind = DataReaderExtensions.GetFieldValue<OrderHistoryItemKind>(reader, "order_history_item_kind"),
                OrderHistoryItemPayload = payload,
            };
        }
    }
}