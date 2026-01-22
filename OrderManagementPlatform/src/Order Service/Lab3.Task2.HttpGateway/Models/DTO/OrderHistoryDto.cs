using Lab3.Task1.Domain.Records;

namespace Lab3.Task2.HttpGateway.Models.DTO;

public class OrderHistoryDto
{
    public long OrderHistoryItemId { get; set; }

    public long OrderId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string Kind { get; set; } = string.Empty;

    public required HistoryItemPayload Payload { get; set; }
}