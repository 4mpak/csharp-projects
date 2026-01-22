using Google.Protobuf.WellKnownTypes;
using Lab3.Task1.Application.Constans.DTO;

namespace Lab3.Task2.Presentation.Extensions;

public static class OrderHistoryMapper
{
    public static OrderHistory MapperToProto(this OrderHistoryDto dto)
    {
        var orderHistoryProto = new OrderHistory
        {
            OrderHistoryItemId = dto.OrderHistoryItemId,
            OrderId = dto.OrderId,
            CreatedAt = Timestamp.FromDateTimeOffset(dto.OrderHistoryItemCreatedAt),
            Kind = dto.OrderHistoryItemKind.MapperToGrpc(),
        };
        switch (dto.OrderHistoryItemPayload)
        {
            case Task1.Domain.Records.HistoryItemPayloadCreated payloadCreated:
                orderHistoryProto.Payload.Created = payloadCreated.MapperToGrpc();
                break;
            case Task1.Domain.Records.HistoryItemPayloadItemAdded payloadItemAdded:
                orderHistoryProto.Payload.ItemAdded = payloadItemAdded.MapperToGrpc();
                break;
            case Task1.Domain.Records.HistoryItemPayloadItemRemoved payloadItemRemoved:
                orderHistoryProto.Payload.ItemRemoved = payloadItemRemoved.MapperToGrpc();
                break;
            case Task1.Domain.Records.HistoryItemPayloadStateChanged payloadStateChanged:
                orderHistoryProto.Payload.StateChanged = payloadStateChanged.MapperToGrpc();
                break;
        }

        return orderHistoryProto;
    }
}