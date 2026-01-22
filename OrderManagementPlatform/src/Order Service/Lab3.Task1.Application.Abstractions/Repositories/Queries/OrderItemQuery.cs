namespace Lab3.Task1.Application.Abstractions.Repositories.Queries;

public record OrderItemQuery(
    long[] OrderIds,
    long[] ProductIds,
    bool? Deleted,
    int Cursor,
    int PageSize);