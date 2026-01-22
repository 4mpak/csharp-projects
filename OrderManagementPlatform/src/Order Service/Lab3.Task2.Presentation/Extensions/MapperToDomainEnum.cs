namespace Lab3.Task2.Presentation.Extensions;

public static class MapperToDomainEnum
{
    public static Task1.Domain.Enums.OrderHistoryItemKind MapperToDomain(this OrderHistoryItemKind item)
    {
        Task1.Domain.Enums.OrderHistoryItemKind domainKind = item switch
        {
            OrderHistoryItemKind.Created => Task1.Domain.Enums.OrderHistoryItemKind.Created,
            OrderHistoryItemKind.ItemAdded => Task1.Domain.Enums.OrderHistoryItemKind.ItemAdded,
            OrderHistoryItemKind.ItemRemoved => Task1.Domain.Enums.OrderHistoryItemKind.ItemRemoved,
            OrderHistoryItemKind.StateChanged => Task1.Domain.Enums.OrderHistoryItemKind.StateChanged,
            OrderHistoryItemKind.Unspecified or _ => throw new ArgumentOutOfRangeException(nameof(item)),
        };

        return domainKind;
    }
}