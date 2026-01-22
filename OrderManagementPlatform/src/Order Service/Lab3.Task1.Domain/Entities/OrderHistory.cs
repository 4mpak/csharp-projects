using Lab3.Task1.Domain.Enums;
using Lab3.Task1.Domain.Records;

namespace Lab3.Task1.Domain.Entities;

public class OrderHistory
{
    public long OrderHistoryItemId { get; set; }

    public long OrderId { get; set; }

    public DateTimeOffset OrderHistoryItemCreatedAt { get; set; }

    public OrderHistoryItemKind OrderHistoryItemKind { get; set; }

    public required HistoryItemPayload OrderHistoryItemPayload { get; init; }
}