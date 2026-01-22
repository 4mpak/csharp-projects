namespace Lab3.Task2.Presentation.Extensions;

public static class HistoryItemPayloadExtension
{
    public static HistoryItemPayloadCreated MapperToGrpc(this Task1.Domain.Records.HistoryItemPayloadCreated payload)
    {
        return new HistoryItemPayloadCreated { CreatedBy = payload.CreatedBy };
    }

    public static HistoryItemPayloadItemAdded MapperToGrpc(
        this Task1.Domain.Records.HistoryItemPayloadItemAdded payload)
    {
        return new HistoryItemPayloadItemAdded { ProductId = payload.Id, Amount = payload.Amount };
    }

    public static HistoryItemPayloadItemRemoved MapperToGrpc(
        this Task1.Domain.Records.HistoryItemPayloadItemRemoved payload)
    {
        return new HistoryItemPayloadItemRemoved { ProductId = payload.Id };
    }

    public static HistoryItemPayloadStateChanged MapperToGrpc(
        this Task1.Domain.Records.HistoryItemPayloadStateChanged payload)
    {
        return new HistoryItemPayloadStateChanged { NewState = payload.NewState };
    }
}