namespace Lab3.Task1.Application.Constans.DTO;

public class ProductQueryDto
{
    public long[]? Ids { get; set; }

    public int MinPrice { get; set; }

    public int MaxPrice { get; set; }

    public string? NamePattern { get; set; }

    public int PageSize { get; set; }

    public int Cursor { get; set; }
}