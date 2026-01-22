namespace Lab3.Task1.Domain.Entities;

public class OrderItem
{
    public long OrderItemId { get; set; }

    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public int OrderItemQuantity { get; set; }

    public bool OrderItemDeleted { get; set; }
}