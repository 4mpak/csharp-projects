namespace Lab3.Task1.Domain.Entities;

public class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}