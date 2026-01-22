using Grpc.Core;
using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task1.Application.Constans.Services;
using Lab3.Task2.Presentation.Extensions;

namespace Lab3.Task2.Presentation.Services;

public class OrderController : OrderServiceProto.OrderServiceProtoBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public override async Task<OrderResponse> CreateOrder(OrderRequest request, ServerCallContext context)
    {
        var dto = new OrderCreatingDto { OrderCreatedBy = request.Name };
        long orderId = await _orderService.CreateOrderAsync(dto);
        return new OrderResponse { OrderId = orderId };
    }

    public override async Task<AddProductResponse> AddProduct(AddProductRequest request, ServerCallContext context)
    {
        long orderId = request.OrderId;
        long productId = request.ProductId;
        int quantity = request.ProductQuantity;
        long orderItemId = await _orderService.AddProductAsync(orderId, productId, quantity);
        return new AddProductResponse { OrderItemId = orderItemId };
    }

    public override async Task<DeleteProductResponse> DeleteProduct(
        DeleteProductRequest request,
        ServerCallContext context)
    {
        long orderId = request.OrderId;
        long productId = request.ProductId;
        long orderItemId = await _orderService.DeleteProductAsync(orderId, productId);
        return new DeleteProductResponse { OrderItemId = orderItemId };
    }

    public override async Task<StateResponse> ChangeStateToProcessing(StateRequest request, ServerCallContext context)
    {
        long orderId = request.OrderId;
        int result = await _orderService.ChangeStateToProcessingAsync(orderId);
        return new StateResponse { OrderId = orderId, Result = result };
    }

    public override async Task<StateResponse> ChangeStateToCompleted(StateRequest request, ServerCallContext context)
    {
        long orderId = request.OrderId;
        int result = await _orderService.ChangeStateToCompletedAsync(orderId);
        return new StateResponse { OrderId = orderId, Result = result };
    }

    public override async Task<StateResponse> ChangeStateToCancelled(StateRequest request, ServerCallContext context)
    {
        long orderId = request.OrderId;
        int result = await _orderService.ChangeStateToCancelledAsync(orderId);
        return new StateResponse { OrderId = orderId, Result = result };
    }

    public override async Task<OrderHistoryResponse> QueryOrderHistory(
        OrderHistoryRequest request,
        ServerCallContext context)
    {
        long orderId = request.OrderId;
        Task1.Domain.Enums.OrderHistoryItemKind? kind = request.Kind.MapperToDomain();
        int pageSize = request.PageSize;
        int cursor = request.Cursor;
        IAsyncEnumerable<OrderHistoryDto> orderHistory = _orderService.QueryOrderHistory(orderId, kind, pageSize, cursor);
        var result = new OrderHistoryResponse();
        await foreach (OrderHistoryDto item in orderHistory)
        {
            result.History.Add(item.MapperToProto());
        }

        return result;
    }
}