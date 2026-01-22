using Lab3.Task1.Domain.Enums;

namespace Lab3.Task1.Domain.Entities;

public class Order
{
    public long OrderId { get; set; }

    public OrderState OrderState { get; set; }

    public DateTimeOffset OrderCreatedAt { get; set; }

    public string OrderCreatedBy { get; set; } = string.Empty;
}