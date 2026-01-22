using Lab3.Task1.Domain.Enums;

namespace Lab3.Task1.Application.Abstractions.Repositories.Queries;

public record OrderHistoryQuery(
    long[] OrderIds,
    OrderHistoryItemKind? HistoryKind,
    int Cursor,
    int PageSize);