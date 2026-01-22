using Lab3.Task1.Domain.Enums;
using Lab3.Task1.Domain.Records;

namespace Lab3.Task1.Application.Constans.DTO;

public class OrderHistoryDto
{
    public required long OrderHistoryItemId { get; init; }

    public required long OrderId { get; init; }

    public required DateTimeOffset OrderHistoryItemCreatedAt { get; init; }

    public required OrderHistoryItemKind OrderHistoryItemKind { get; init; }

    public required HistoryItemPayload OrderHistoryItemPayload { get; init; }
}