namespace Lab3.Task1.Application.Abstractions.Repositories.Queries;

public record ProductQuery(
    long[]? Ids,
    int? MaxPrice,
    int? MinPrice,
    string? NamePattern,
    int Cursor,
    int PageSize);