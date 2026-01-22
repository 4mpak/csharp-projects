using Lab3.Task2.HttpGateway.Extensions;
using Lab3.Task2.HttpGateway.Mappers;
using Lab3.Task2.HttpGateway.Models.AddProduct;
using Lab3.Task2.HttpGateway.Models.ChangeState;
using Lab3.Task2.HttpGateway.Models.CreateOrder;
using Lab3.Task2.HttpGateway.Models.DeleteProduct;
using Lab3.Task2.HttpGateway.Models.DTO;
using Lab3.Task2.HttpGateway.Models.KafkaDto;
using Lab3.Task2.HttpGateway.Services.Interfaces;
using Lab3.Task2.Presentation;
using Orders.ProcessingService.Contracts;
using OrderHistoryItemKind = Lab3.Task1.Domain.Enums.OrderHistoryItemKind;

namespace Lab3.Task2.HttpGateway.Services;

public class OrderGatewayService : IOrderGatewayService
{
    private readonly OrderServiceProto.OrderServiceProtoClient _client;
    private readonly OrderService.OrderServiceClient _processingClient;
    private readonly OrderMapper _mapper;

    public OrderGatewayService(OrderServiceProto.OrderServiceProtoClient client, OrderService.OrderServiceClient processingClient, OrderMapper mapper)
    {
        _client = client;
        _processingClient = processingClient;
        _mapper = mapper;
    }

    public async Task<CreateOrderReply> CreateOrderAsync(CreateOrderRequest request)
    {
        var grpcRequest = new OrderRequest { Name = request.CreatedBy };
        OrderResponse reply = await _client.CreateOrderAsync(grpcRequest);
        return new CreateOrderReply(reply.OrderId);
    }

    public async Task<AddProductReplyDto> AddProductAsync(AddProductRequestDto request)
    {
        var grpcRequest = new AddProductRequest { OrderId = request.OrderId, ProductId = request.ProductId, ProductQuantity = request.Quantity };
        AddProductResponse reply = await _client.AddProductAsync(grpcRequest);
        return new AddProductReplyDto(reply.OrderItemId);
    }

    public async Task<DeleteProductReplyDto> DeleteProductAsync(DeleteProductRequestDto request)
    {
        var grpcRequest = new DeleteProductRequest { OrderId = request.OrderId, ProductId = request.ProductId };
        DeleteProductResponse reply = await _client.DeleteProductAsync(grpcRequest);
        return new DeleteProductReplyDto(reply.OrderItemId);
    }

    public async Task<StateReplyDto> ChangeToProcessingAsync(long orderId)
    {
        var grpcRequest = new StateRequest { OrderId = orderId };
        StateResponse reply = await _client.ChangeStateToProcessingAsync(grpcRequest);
        return new StateReplyDto(reply.OrderId, reply.Result);
    }

    public async Task<StateReplyDto> ChangeToCompletedAsync(long orderId)
    {
        var grpcRequest = new StateRequest { OrderId = orderId };
        StateResponse reply = await _client.ChangeStateToCompletedAsync(grpcRequest);
        return new StateReplyDto(reply.OrderId, reply.Result);
    }

    public async Task<StateReplyDto> ChangeToCancelledAsync(long orderId)
    {
        var grpcRequest = new StateRequest { OrderId = orderId };
        StateResponse reply = await _client.ChangeStateToCancelledAsync(grpcRequest);
        return new StateReplyDto(reply.OrderId, reply.Result);
    }

    public async Task<IEnumerable<OrderHistoryDto>> QueryOrderHistoryAsync(
        long orderId,
        int cursor,
        int pageSize,
        OrderHistoryItemKind kind)
    {
        var grpcRequest = new OrderHistoryRequest
        {
            OrderId = orderId,
            PageSize = pageSize,
            Cursor = cursor,
            Kind = kind.MapperToGrpc(),
        };
        OrderHistoryResponse reply = await _client.QueryOrderHistoryAsync(grpcRequest);
        var result = new List<OrderHistoryDto>();
        foreach (OrderHistory? historyItem in reply.History)
        {
            OrderHistoryDto mappedItem = _mapper.MapperFromGrpc(historyItem);
            result.Add(mappedItem);
        }

        return result;
    }

    public async Task<ApproveOrderResponse> ApproveOrderAsync(long orderId, ApproveOrderDto dto)
    {
        var request = new ApproveOrderRequest
        {
            OrderId = orderId,
            FailureReason = dto.FailureReason,
            IsApproved = dto.IsApproved,
            ApprovedBy = dto.ApprovedBy,
        };
        ApproveOrderResponse response = await _processingClient.ApproveOrderAsync(request);
        return response;
    }

    public async Task<StartOrderPackingResponse> StartOrderPackingAsync(long orderId, StartOrderPackingDto dto)
    {
        var request = new StartOrderPackingRequest
        {
            OrderId = orderId,
            PackingBy = dto.PackingBy,
        };
        StartOrderPackingResponse response = await _processingClient.StartOrderPackingAsync(request);
        return response;
    }

    public async Task<FinishOrderPackingResponse> FinishOrderPackingAsync(long orderId, FinishOrderPackingDto dto)
    {
        var request = new FinishOrderPackingRequest
        {
            OrderId = orderId,
            FailureReason = dto.FailureReason,
            IsSuccessful = dto.IsSuccessful,
        };
        FinishOrderPackingResponse response = await _processingClient.FinishOrderPackingAsync(request);
        return response;
    }

    public async Task<StartOrderDeliveryResponse> StartOrderDeliveryAsync(long orderId, StartOrderDeliveryDto dto)
    {
        var request = new StartOrderDeliveryRequest
        {
            OrderId = orderId,
            DeliveredBy = dto.DeliveredBy,
        };
        StartOrderDeliveryResponse response = await _processingClient.StartOrderDeliveryAsync(request);
        return response;
    }

    public async Task<FinishOrderDeliveryResponse> FinishOrderDeliveryAsync(long orderId, FinishOrderDeliveryDto dto)
    {
        var request = new FinishOrderDeliveryRequest
        {
            OrderId = orderId,
            FailureReason = dto.FailureReason,
            IsSuccessful = dto.IsSaccessful,
        };
        FinishOrderDeliveryResponse response = await _processingClient.FinishOrderDeliveryAsync(request);
        return response;
    }
}