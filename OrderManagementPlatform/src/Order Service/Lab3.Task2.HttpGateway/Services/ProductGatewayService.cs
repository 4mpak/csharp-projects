using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task2.HttpGateway.Models.CreateProduct;
using Lab3.Task2.HttpGateway.Services.Interfaces;
using Lab3.Task2.Presentation;

namespace Lab3.Task2.HttpGateway.Services;

public class ProductGatewayService : IProductGatewayService
{
    private readonly ProductServiceProto.ProductServiceProtoClient _client;

    public ProductGatewayService(ProductServiceProto.ProductServiceProtoClient client)
    {
        _client = client;
    }

    public async Task<CreateProductReplyDto> CreateProductAsync(ProductCreatingDto dto)
    {
        var grpcRequest = new ProductRequest { Name = dto.Name, Price = (int)dto.Price };
        ProductResponse reply = await _client.CreateProductAsync(grpcRequest);
        return new CreateProductReplyDto { Id = reply.ProductId };
    }

    public async Task<IEnumerable<ProductDto>> GetProductAsync(ProductQueryDto dto)
    {
        var grpcRequest = new GetProductsRequest { PageSize = dto.PageSize, Cursor = dto.Cursor };
        grpcRequest.Ids.Add(dto.Ids);
        grpcRequest.MinPrice = dto.MinPrice;
        grpcRequest.MaxPrice = dto.MaxPrice;
        grpcRequest.NamePattern = dto.NamePattern;
        GetProductsResponse reply = await _client.GetProductsAsync(grpcRequest);
        var result = new List<ProductDto>();
        foreach (ProductProto? product in reply.Products)
        {
            result.Add(new ProductDto
            {
                Id = product.ProductId,
                Name = product.Name,
                Price = decimal.Parse(product.Price),
            });
        }

        return result;
    }
}