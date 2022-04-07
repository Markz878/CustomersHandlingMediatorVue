namespace Core.Handlers.Order;
public class AddTomatoesToEveryThirdOrderForCustomerCommand : IRequest<bool>
{
    public Guid CustomerId { get; }

    public AddTomatoesToEveryThirdOrderForCustomerCommand(Guid customerId)
    {
        CustomerId = customerId;
    }
}

internal class AddTomatoesToEveryThirdOrderForCustomerCommandHandler : IRequestHandler<AddTomatoesToEveryThirdOrderForCustomerCommand, bool>
{
    private readonly AppDbContext db;

    public AddTomatoesToEveryThirdOrderForCustomerCommandHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<bool> Handle(AddTomatoesToEveryThirdOrderForCustomerCommand request, CancellationToken cancellationToken)
    {
        CustomerDb? customer = await db.Customers.Include(x=>x.Orders).FirstOrDefaultAsync(x=>x.Id == request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return false;
        }
        AddTomatoesToEveryThirdOrder(customer.Orders);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private void AddTomatoesToEveryThirdOrder(List<OrderDb> orders)
    {
        for (int i = 0; i < orders.Count; i+=3)
        {
            orders[i].Items.Add(new OrderItemDb()
            {
                Name = "Tomato",
                Amount = 3,
                Price = 2
            });
        }
    }
}
