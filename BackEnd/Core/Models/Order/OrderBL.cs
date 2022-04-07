namespace Core.Models.Order;

public class OrderBL
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderPlacedTime { get; set; }
    public OrderState OrderState { get; set; }
    public ICollection<OrderItemBL> Items { get; set; } = new List<OrderItemBL>();
}
