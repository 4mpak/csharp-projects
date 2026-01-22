using Lab3.Task1.Domain.Enums;
using Lab3.Task1.Domain.Records;

namespace Lab3.Task1.Application.Constans.DTO;

public class HistoryCreatingDto
{
    public required DateTimeOffset CreatedAt { get; init; }

    public required OrderHistoryItemKind Kind { get; init; }

    public required HistoryItemPayload Payload { get; init; }
}