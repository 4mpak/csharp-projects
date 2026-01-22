using Lab3.Task2.Presentation;

namespace Lab3.Task2.HttpGateway.Extensions;

public static class MapperToGrpcEnum
{
    public static OrderHistoryItemKind MapperToGrpc(this Task1.Domain.Enums.OrderHistoryItemKind item)
    {
        OrderHistoryItemKind kind = item switch
        {
            Task1.Domain.Enums.OrderHistoryItemKind.Created => OrderHistoryItemKind.Created,
            Task1.Domain.Enums.OrderHistoryItemKind.ItemAdded => OrderHistoryItemKind.ItemAdded,
            Task1.Domain.Enums.OrderHistoryItemKind.ItemRemoved => OrderHistoryItemKind.ItemRemoved,
            Task1.Domain.Enums.OrderHistoryItemKind.StateChanged => OrderHistoryItemKind.StateChanged,
            _ => OrderHistoryItemKind.Unspecified,
        };
        return kind;
    }
}