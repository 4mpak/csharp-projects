using Lab3.Task1.Domain.Enums;
using Lab3.Task2.HttpGateway.Models.AddProduct;
using Lab3.Task2.HttpGateway.Models.ChangeState;
using Lab3.Task2.HttpGateway.Models.CreateOrder;
using Lab3.Task2.HttpGateway.Models.DeleteProduct;
using Lab3.Task2.HttpGateway.Models.DTO;
using Lab3.Task2.HttpGateway.Models.KafkaDto;
using Orders.ProcessingService.Contracts;

namespace Lab3.Task2.HttpGateway.Services.Interfaces;

public interface IOrderGatewayService
{
    Task<CreateOrderReply> CreateOrderAsync(CreateOrderRequest request);

    Task<AddProductReplyDto> AddProductAsync(AddProductRequestDto request);

    Task<DeleteProductReplyDto> DeleteProductAsync(DeleteProductRequestDto request);

    Task<StateReplyDto> ChangeToProcessingAsync(long orderId);

    Task<StateReplyDto> ChangeToCompletedAsync(long orderId);

    Task<StateReplyDto> ChangeToCancelledAsync(long orderId);

    Task<IEnumerable<OrderHistoryDto>> QueryOrderHistoryAsync(
        long orderId, int cursor, int pageSize, OrderHistoryItemKind kind);

    Task<ApproveOrderResponse> ApproveOrderAsync(long orderId, ApproveOrderDto dto);

    Task<StartOrderPackingResponse> StartOrderPackingAsync(long orderId, StartOrderPackingDto dto);

    Task<FinishOrderPackingResponse> FinishOrderPackingAsync(long orderId, FinishOrderPackingDto dto);

    Task<StartOrderDeliveryResponse> StartOrderDeliveryAsync(long orderId, StartOrderDeliveryDto dto);

    Task<FinishOrderDeliveryResponse> FinishOrderDeliveryAsync(long orderId, FinishOrderDeliveryDto dto);
}