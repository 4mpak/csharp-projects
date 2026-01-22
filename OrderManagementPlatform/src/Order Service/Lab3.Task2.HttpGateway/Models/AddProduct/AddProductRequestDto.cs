namespace Lab3.Task2.HttpGateway.Models.AddProduct;

public record AddProductRequestDto(long OrderId, long ProductId, int Quantity);