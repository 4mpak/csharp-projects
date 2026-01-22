using Google.Protobuf.WellKnownTypes;
using Lab3.Task1.Application.Abstractions.Repositories;
using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task1.Application.Constans.Services;
using Lab3.Task1.Domain.Entities;
using Lab3.Task1.Domain.Enums;
using Lab3.Task1.Domain.Records;
using Lab4.Task1.Kafka.Message;
using Lab4.Task1.Kafka.Producers;
using Orders.Kafka.Contracts;
using System.Transactions;

namespace Lab3.Task1.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderHistoryRepository _orderHistoryRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IKafkaProducer<OrderCreationKey, OrderCreationValue> _kafkaProducer;

    private static TransactionScope Create(IsolationLevel isolationLevel)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = isolationLevel,
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public OrderService(IOrderRepository orderRepository, IOrderHistoryRepository orderHistoryRepository, IOrderItemRepository orderItemRepository, IKafkaProducer<OrderCreationKey, OrderCreationValue> kafkaProducer)
    {
        _orderRepository = orderRepository;
        _orderHistoryRepository = orderHistoryRepository;
        _orderItemRepository = orderItemRepository;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<long> CreateOrderAsync(OrderCreatingDto dto)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        var order = new Order
        {
            OrderCreatedAt = DateTimeOffset.Now.UtcDateTime,
            OrderCreatedBy = dto.OrderCreatedBy,
            OrderState = OrderState.Created,
        };
        long orderId = await _orderRepository.CreateOrderAsync(order);

        var orderHistory = new OrderHistory
        {
            OrderId = orderId,
            OrderHistoryItemCreatedAt = DateTimeOffset.Now.UtcDateTime,
            OrderHistoryItemKind = OrderHistoryItemKind.Created,
            OrderHistoryItemPayload = new HistoryItemPayloadCreated(dto.OrderCreatedBy),
        };
        await _orderHistoryRepository.CreateOrderHistoryAsync(orderHistory);

        var orderCreationMessage =
            new ProducerMessage<OrderCreationKey, OrderCreationValue>(
                new OrderCreationKey { OrderId = orderId },
                new OrderCreationValue
                {
                    OrderCreated = new OrderCreationValue.Types.OrderCreated { OrderId = orderId, CreatedAt = order.OrderCreatedAt.ToTimestamp() },
                });

        await _kafkaProducer.ProduceAsync(orderCreationMessage, CancellationToken.None);
        transaction.Complete();
        return orderId;
    }

    public async Task<long> AddProductAsync(long orderId, long productId, int quantity)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        var orderItem = new OrderItem
        {
            OrderId = orderId,
            OrderItemDeleted = false,
            ProductId = productId,
            OrderItemQuantity = quantity,
        };
        long orderItemId = await _orderItemRepository.CreateOrderItemAsync(orderItem);

        var orderHistory = new OrderHistory
        {
            OrderId = orderId,
            OrderHistoryItemCreatedAt = DateTimeOffset.Now.UtcDateTime,
            OrderHistoryItemKind = OrderHistoryItemKind.ItemAdded,
            OrderHistoryItemPayload = new HistoryItemPayloadItemAdded(productId, quantity),
        };
        await _orderHistoryRepository.CreateOrderHistoryAsync(orderHistory);
        transaction.Complete();
        return orderItemId;
    }

    public async Task<int> DeleteProductAsync(long orderId, long productId)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        int answer = await _orderItemRepository.DeleteWithProductIdAsync(orderId, productId);
        var orderHistory = new OrderHistory
        {
            OrderId = orderId,
            OrderHistoryItemCreatedAt = DateTimeOffset.Now.UtcDateTime,
            OrderHistoryItemKind = OrderHistoryItemKind.ItemRemoved,
            OrderHistoryItemPayload = new HistoryItemPayloadItemRemoved(productId),
        };
        await _orderHistoryRepository.CreateOrderHistoryAsync(orderHistory);
        transaction.Complete();
        return answer;
    }

    public async Task<int> ChangeStateToProcessingAsync(long orderId)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        int answer = await _orderRepository.ChangeOrderStateAsync(orderId, OrderState.Processing);

        await _orderHistoryRepository.CreateOrderHistoryAsync(new OrderHistory
        {
            OrderId = orderId,
            OrderHistoryItemCreatedAt = DateTimeOffset.Now.UtcDateTime,
            OrderHistoryItemKind = OrderHistoryItemKind.StateChanged,
            OrderHistoryItemPayload = new HistoryItemPayloadStateChanged(OrderState.Processing.ToString()),
        });

        var message =
            new ProducerMessage<OrderCreationKey, OrderCreationValue>(
                new OrderCreationKey { OrderId = orderId },
                new OrderCreationValue
                {
                    OrderProcessingStarted = new OrderCreationValue.Types.OrderProcessingStarted { OrderId = orderId, StartedAt = DateTimeOffset.Now.UtcDateTime.ToTimestamp() },
                });

        transaction.Complete();
        return answer;
    }

    public async Task<int> ChangeStateToCompletedAsync(long orderId)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        int answer = await _orderRepository.ChangeOrderStateAsync(orderId, OrderState.Completed);

        await _orderHistoryRepository.CreateOrderHistoryAsync(new OrderHistory
        {
            OrderId = orderId,
            OrderHistoryItemCreatedAt = DateTimeOffset.Now.UtcDateTime,
            OrderHistoryItemKind = OrderHistoryItemKind.StateChanged,
            OrderHistoryItemPayload = new HistoryItemPayloadStateChanged(OrderState.Completed.ToString()),
        });
        transaction.Complete();
        return answer;
    }

    public async Task<int> ChangeStateToCancelledAsync(long orderId)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        int answer = await _orderRepository.ChangeOrderStateAsync(orderId, OrderState.Cancelled);
        await _orderHistoryRepository.CreateOrderHistoryAsync(new OrderHistory
        {
            OrderId = orderId,
            OrderHistoryItemCreatedAt = DateTimeOffset.Now.UtcDateTime,
            OrderHistoryItemKind = OrderHistoryItemKind.StateChanged,
            OrderHistoryItemPayload = new HistoryItemPayloadStateChanged(OrderState.Cancelled.ToString()),
        });
        transaction.Complete();
        return answer;
    }

    public async IAsyncEnumerable<OrderHistoryDto> QueryOrderHistory(long orderId, OrderHistoryItemKind? kind, int pageSize, int cursor)
    {
        using TransactionScope transaction = Create(IsolationLevel.ReadCommitted);

        IAsyncEnumerable<OrderHistory> orderHistory = _orderHistoryRepository.QueryOrderHistoryAsync(
            new OrderHistoryQuery([orderId], kind, cursor, pageSize));

        await foreach (OrderHistory item in orderHistory)
        {
            yield return new OrderHistoryDto
            {
                OrderHistoryItemId = item.OrderHistoryItemId,
                OrderId = item.OrderId,
                OrderHistoryItemCreatedAt = item.OrderHistoryItemCreatedAt,
                OrderHistoryItemKind = item.OrderHistoryItemKind,
                OrderHistoryItemPayload = item.OrderHistoryItemPayload,
            };
        }

        transaction.Complete();
    }

    public async Task AddOrderProcessingHistory(long orderId, HistoryCreatingDto dto)
    {
        await _orderHistoryRepository.CreateOrderHistoryAsync(new OrderHistory
        {
            OrderId = orderId,
            OrderHistoryItemCreatedAt = dto.CreatedAt,
            OrderHistoryItemKind = dto.Kind,
            OrderHistoryItemPayload = dto.Payload,
        });
    }
}