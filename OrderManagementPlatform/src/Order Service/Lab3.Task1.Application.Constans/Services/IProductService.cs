using Lab3.Task1.Application.Constans.DTO;

namespace Lab3.Task1.Application.Constans.Services;

public interface IProductService
{
    Task<long> CreateProductAsync(ProductCreatingDto dto);

    Task<IEnumerable<ProductDto>> GetProductAsync(ProductQueryDto dto);
}