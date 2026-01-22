using Lab3.Task1.Application.Abstractions.Repositories;
using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task1.Application.Constans.Services;
using Lab3.Task1.Domain.Entities;
using System.Transactions;

namespace Lab3.Task1.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    private static TransactionScope Create(IsolationLevel isolationLevel)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = isolationLevel,
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> CreateProductAsync(ProductCreatingDto dto)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
        };
        long id = await _repository.CreateProductAsync(product);

        transaction.Complete();
        return id;
    }

    public async Task<IEnumerable<ProductDto>> GetProductAsync(ProductQueryDto dto)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        var query = new ProductQuery(dto.Ids, dto.MaxPrice, dto.MinPrice, dto.NamePattern, dto.PageSize, dto.Cursor);
        IAsyncEnumerable<Product> products = _repository.QueryProductAsync(query);
        var result = new List<ProductDto>();
        await foreach (Product product in products)
        {
            result.Add(new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
            });
        }

        transaction.Complete();
        return result;
    }
}