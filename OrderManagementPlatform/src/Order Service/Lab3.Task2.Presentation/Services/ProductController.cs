using Grpc.Core;
using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task1.Application.Constans.Services;
using System.Globalization;

namespace Lab3.Task2.Presentation.Services;

public class ProductController : ProductServiceProto.ProductServiceProtoBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task<ProductResponse> CreateProduct(ProductRequest request, ServerCallContext context)
    {
        var dto = new ProductCreatingDto { Name = request.Name, Price = request.Price };
        long productId = await _productService.CreateProductAsync(dto);
        return new ProductResponse { ProductId = productId };
    }

    public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
    {
        var dto = new ProductQueryDto
        {
            Ids = request.Ids.ToArray(),
            MaxPrice = request.MaxPrice,
            MinPrice = request.MinPrice,
            NamePattern = request.NamePattern,
            PageSize = request.PageSize,
            Cursor = request.Cursor,
        };
        IEnumerable<ProductDto> result = await _productService.GetProductAsync(dto);
        var reply = new GetProductsResponse();
        foreach (ProductDto product in result)
        {
            reply.Products.Add(new ProductProto
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price.ToString(CultureInfo.InvariantCulture),
            });
        }

        return reply;
    }
}