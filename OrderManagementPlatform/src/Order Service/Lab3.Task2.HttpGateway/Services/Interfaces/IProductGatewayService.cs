using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task2.HttpGateway.Models.CreateProduct;

namespace Lab3.Task2.HttpGateway.Services.Interfaces;

public interface IProductGatewayService
{
    Task<CreateProductReplyDto> CreateProductAsync(ProductCreatingDto dto);

    Task<IEnumerable<ProductDto>> GetProductAsync(ProductQueryDto dto);
}