namespace Core.Models.Order;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public DateTime OrderPlacedTime { get; set; }
    public OrderState OrderState { get; set; }
    public List<OrderItemBL> Items { get; set; } = new List<OrderItemBL>();
}