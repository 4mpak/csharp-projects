using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task2.HttpGateway.Models.CreateProduct;
using Lab3.Task2.HttpGateway.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Task2.HttpGateway.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductGatewayService _service;

    public ProductController(IProductGatewayService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateProductAsync([FromBody] ProductCreatingDto dto)
    {
        CreateProductReplyDto result = await _service.CreateProductAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsAsync([FromBody] ProductQueryDto dto)
    {
        IEnumerable<ProductDto> result = await _service.GetProductAsync(dto);
        return Ok(result);
    }
}