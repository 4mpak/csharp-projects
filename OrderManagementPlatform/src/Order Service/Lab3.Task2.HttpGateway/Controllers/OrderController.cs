using Lab3.Task2.HttpGateway.Models.AddProduct;
using Lab3.Task2.HttpGateway.Models.ChangeState;
using Lab3.Task2.HttpGateway.Models.CreateOrder;
using Lab3.Task2.HttpGateway.Models.DeleteProduct;
using Lab3.Task2.HttpGateway.Models.DTO;
using Lab3.Task2.HttpGateway.Models.KafkaDto;
using Lab3.Task2.HttpGateway.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.ProcessingService.Contracts;
using OrderHistoryItemKind = Lab3.Task1.Domain.Enums.OrderHistoryItemKind;

namespace Lab3.Task2.HttpGateway.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderGatewayService _service;

    public OrderController(IOrderGatewayService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
    {
        CreateOrderReply result = await _service.CreateOrderAsync(request);
        return Ok(result);
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddProduct([FromBody] AddProductRequestDto request)
    {
        AddProductReplyDto result = await _service.AddProductAsync(request);
        return Ok(result);
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteProduct([FromBody] DeleteProductRequestDto request)
    {
        DeleteProductReplyDto result = await _service.DeleteProductAsync(request);
        return Ok(result);
    }

    [HttpPatch("{orderId}/to_processing")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ChangeToProcessing(long orderId)
    {
        StateReplyDto result = await _service.ChangeToProcessingAsync(orderId);
        return Ok(result);
    }

    [HttpPatch("{orderId}/to_completed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ChangeToCompleted(long orderId)
    {
        StateReplyDto result = await _service.ChangeToCompletedAsync(orderId);
        return Ok(result);
    }

    [HttpPatch("{orderId}/to_cancelled")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ChangeToCancelled(long orderId)
    {
        StateReplyDto result = await _service.ChangeToCancelledAsync(orderId);
        return Ok(result);
    }

    [HttpGet("{orderId}/history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<OrderHistoryDto>>> GetOrderHistory(
        [FromRoute] long orderId,
        [FromQuery] int cursor,
        [FromQuery] int pageSize,
        [FromQuery] OrderHistoryItemKind kind)
    {
        IEnumerable<OrderHistoryDto> result = await _service.QueryOrderHistoryAsync(orderId, cursor, pageSize, kind);
        return Ok(result);
    }

    [HttpPatch("{orderId}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ApproveOrder(
        [FromRoute] long orderId,
        [FromBody] ApproveOrderDto dto)
    {
        ApproveOrderResponse response = await _service.ApproveOrderAsync(orderId, dto);
        return Ok(response);
    }

    [HttpPatch("{orderId}/start-packing")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> StartOrderPacking(
        [FromRoute] long orderId,
        [FromBody] StartOrderPackingDto dto)
    {
        StartOrderPackingResponse response = await _service.StartOrderPackingAsync(orderId, dto);
        return Ok(response);
    }

    [HttpPatch("{orderId}/finish-packing")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> FinishOrderPacking(
        [FromRoute] long orderId,
        [FromBody] FinishOrderPackingDto dto)
    {
        FinishOrderPackingResponse response = await _service.FinishOrderPackingAsync(orderId, dto);
        return Ok(response);
    }

    [HttpPatch("{orderId}/start-delivery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> StartOrderDelivery(
        [FromRoute] long orderId,
        [FromBody] StartOrderDeliveryDto dto)
    {
        StartOrderDeliveryResponse response = await _service.StartOrderDeliveryAsync(orderId, dto);
        return Ok(response);
    }

    [HttpPatch("{orderId}/finish-delivery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> FinishOrderDelivery(
        [FromRoute] long orderId,
        [FromBody] FinishOrderDeliveryDto dto)
    {
        FinishOrderDeliveryResponse response = await _service.FinishOrderDeliveryAsync(orderId, dto);
        return Ok(response);
    }
}