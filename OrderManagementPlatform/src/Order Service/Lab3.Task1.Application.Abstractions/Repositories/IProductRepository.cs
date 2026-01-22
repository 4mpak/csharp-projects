using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;

namespace Lab3.Task1.Application.Abstractions.Repositories;

public interface IProductRepository
{
    Task<long> CreateProductAsync(Product product);

    IAsyncEnumerable<Product> QueryProductAsync(ProductQuery query);
}