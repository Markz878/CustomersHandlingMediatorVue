namespace Core.Data.Models;

public class OrderDb
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerDb? Customer { get; set; }
    public DateTime OrderPlacedTime { get; set; }
    public OrderState State { get; set; }
    public ICollection<OrderItemDb> Items { get; set; } = new List<OrderItemDb>();
}
