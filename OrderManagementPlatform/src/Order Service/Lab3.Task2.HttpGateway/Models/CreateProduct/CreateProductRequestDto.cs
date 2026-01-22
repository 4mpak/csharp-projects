namespace Lab3.Task2.HttpGateway.Models.CreateProduct;

public class CreateProductRequestDto
{
    public string Name { get; set; } = string.Empty;

    public int Price { get; set; }
}