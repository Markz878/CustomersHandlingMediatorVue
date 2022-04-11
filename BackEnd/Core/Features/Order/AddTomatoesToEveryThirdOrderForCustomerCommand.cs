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
        CustomerDb? customer = await db.Customers.Include(x => x.Orders).FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null)
        {
            return false;
        }
        CustomerBL customerBL = customer.Adapt<CustomerBL>();
        customerBL.AddTomatoesToEveryThirdOrder();
        db.Customers.Update(customerBL.Adapt<CustomerDb>());
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
