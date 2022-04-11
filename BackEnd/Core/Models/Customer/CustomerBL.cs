namespace Core.Models.Customer;

public class CustomerBL
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public List<OrderBL> Orders { get; set; } = new List<OrderBL>();

    public void AddTomatoesToEveryThirdOrder()
    {
        for (int i = 0; i < Orders.Count; i += 3)
        {
            Orders[i].Items.Add(new OrderItemBL()
            {
                Name = "Tomato",
                Amount = 3,
                Price = 2
            });
        }
    }
}
