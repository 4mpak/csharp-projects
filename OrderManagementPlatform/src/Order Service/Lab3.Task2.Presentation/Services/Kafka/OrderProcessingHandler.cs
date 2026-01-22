using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task1.Application.Constans.Services;
using Lab3.Task2.Presentation.Services.Kafka.Interfaces;
using Lab4.Task1.Kafka.Message;
using Orders.Kafka.Contracts;

namespace Lab3.Task2.Presentation.Services.Kafka;

public class OrderProcessingHandler : IKafkaMessageBatch<OrderProcessingKey, OrderProcessingValue>
{
    private readonly IOrderService _orderService;

    public OrderProcessingHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task HandleBatchMessage(
        IEnumerable<ConsumerMessage<OrderProcessingKey, OrderProcessingValue>> message,
        CancellationToken cancellationToken)
    {
        foreach (ConsumerMessage<OrderProcessingKey, OrderProcessingValue> mess in message)
        {
            switch (mess.Value.EventCase)
            {
                case OrderProcessingValue.EventOneofCase.ApprovalReceived:
                    if (!mess.Value.ApprovalReceived.IsApproved)
                    {
                        await _orderService.ChangeStateToCancelledAsync(mess.Key.OrderId);
                        break;
                    }

                    var dto1 = new HistoryCreatingDto
                    {
                        Kind = Task1.Domain.Enums.OrderHistoryItemKind.StateChanged,
                        CreatedAt = mess.Value.ApprovalReceived.CreatedAt.ToDateTimeOffset(),
                        Payload = new Task1.Domain.Records.HistoryItemPayloadStateChanged("approval received - Kafka"),
                    };
                    await _orderService.AddOrderProcessingHistory(mess.Key.OrderId, dto1);
                    break;

                case OrderProcessingValue.EventOneofCase.PackingStarted:
                    var dto2 = new HistoryCreatingDto
                    {
                        Kind = Task1.Domain.Enums.OrderHistoryItemKind.StateChanged,
                        CreatedAt = mess.Value.PackingStarted.StartedAt.ToDateTimeOffset(),
                        Payload = new Task1.Domain.Records.HistoryItemPayloadStateChanged("packing started - Kafka"),
                    };
                    await _orderService.AddOrderProcessingHistory(mess.Key.OrderId, dto2);
                    break;

                case OrderProcessingValue.EventOneofCase.PackingFinished:
                    if (!mess.Value.PackingFinished.IsFinishedSuccessfully)
                    {
                        await _orderService.ChangeStateToCancelledAsync(mess.Key.OrderId);
                        break;
                    }

                    var dto3 = new HistoryCreatingDto
                    {
                        Kind = Task1.Domain.Enums.OrderHistoryItemKind.StateChanged,
                        CreatedAt = mess.Value.PackingFinished.FinishedAt.ToDateTimeOffset(),
                        Payload = new Task1.Domain.Records.HistoryItemPayloadStateChanged("packing finished - Kafka"),
                    };
                    await _orderService.AddOrderProcessingHistory(mess.Key.OrderId, dto3);
                    break;

                case OrderProcessingValue.EventOneofCase.DeliveryStarted:
                    var dto4 = new HistoryCreatingDto
                    {
                        Kind = Task1.Domain.Enums.OrderHistoryItemKind.StateChanged,
                        CreatedAt = mess.Value.DeliveryStarted.StartedAt.ToDateTimeOffset(),
                        Payload = new Task1.Domain.Records.HistoryItemPayloadStateChanged("delivery started - Kafka"),
                    };
                    await _orderService.AddOrderProcessingHistory(mess.Key.OrderId, dto4);
                    break;

                case OrderProcessingValue.EventOneofCase.DeliveryFinished:
                    if (!mess.Value.DeliveryFinished.IsFinishedSuccessfully)
                    {
                        await _orderService.ChangeStateToCancelledAsync(mess.Key.OrderId);
                        break;
                    }

                    var dto5 = new HistoryCreatingDto
                    {
                        Kind = Task1.Domain.Enums.OrderHistoryItemKind.StateChanged,
                        CreatedAt = mess.Value.DeliveryFinished.FinishedAt.ToDateTimeOffset(),
                        Payload = new Task1.Domain.Records.HistoryItemPayloadStateChanged("delivery finished - Kafka"),
                    };
                    await _orderService.AddOrderProcessingHistory(mess.Key.OrderId, dto5);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("message");
            }
        }
    }
}