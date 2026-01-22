using Lab3.Task1.Domain.Records;
using Lab3.Task2.HttpGateway.Models.DTO;
using Lab3.Task2.Presentation;
using HistoryItemPayloadCreated = Lab3.Task1.Domain.Records.HistoryItemPayloadCreated;
using HistoryItemPayloadItemAdded = Lab3.Task1.Domain.Records.HistoryItemPayloadItemAdded;
using HistoryItemPayloadItemRemoved = Lab3.Task1.Domain.Records.HistoryItemPayloadItemRemoved;
using HistoryItemPayloadStateChanged = Lab3.Task1.Domain.Records.HistoryItemPayloadStateChanged;

namespace Lab3.Task2.HttpGateway.Mappers;

public class OrderMapper
{
    public OrderHistoryDto MapperFromGrpc(OrderHistory orderHistory)
    {
        HistoryItemPayload payload = orderHistory.Payload.PayloadCase switch
        {
            OrderHistoryPayload.PayloadOneofCase.Created
                => new HistoryItemPayloadCreated(orderHistory.Payload.Created.CreatedBy),
            OrderHistoryPayload.PayloadOneofCase.ItemAdded
                => new HistoryItemPayloadItemAdded(
                    orderHistory.Payload.ItemAdded.ProductId,
                    orderHistory.Payload.ItemAdded.Amount),
            OrderHistoryPayload.PayloadOneofCase.ItemRemoved
                => new HistoryItemPayloadItemRemoved(orderHistory.Payload.ItemRemoved.ProductId),
            OrderHistoryPayload.PayloadOneofCase.StateChanged
                => new HistoryItemPayloadStateChanged(orderHistory.Payload.StateChanged.NewState),
            OrderHistoryPayload.PayloadOneofCase.None or _
                => throw new ArgumentOutOfRangeException(nameof(orderHistory)),
        };

        return new OrderHistoryDto
        {
            OrderHistoryItemId = orderHistory.OrderHistoryItemId,
            OrderId = orderHistory.OrderId,
            CreatedAt = orderHistory.CreatedAt.ToDateTimeOffset(),
            Kind = orderHistory.Kind.ToString(),
            Payload = payload,
        };
    }
}