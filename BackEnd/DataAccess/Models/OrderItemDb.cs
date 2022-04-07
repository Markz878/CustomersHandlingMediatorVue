namespace DataAccess.Models;

public class OrderItemDb
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public Guid OrderId { get; set; }
    public OrderDb? Order { get; set; }
}
