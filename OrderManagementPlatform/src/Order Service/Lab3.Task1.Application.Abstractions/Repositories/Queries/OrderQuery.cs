using Lab3.Task1.Domain.Enums;

namespace Lab3.Task1.Application.Abstractions.Repositories.Queries;

public record OrderQuery(
    long[] Ids,
    OrderState? State,
    string? OrderCreatedBy,
    int Cursor,
    int PageSize);