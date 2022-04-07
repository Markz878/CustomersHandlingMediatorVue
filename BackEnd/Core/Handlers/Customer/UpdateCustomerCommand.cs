namespace Core.Handlers.Customer;

public class UpdateCustomerCommand : IRequest<bool>
{
    public CustomerBL CustomerRequest { get; }
    public UpdateCustomerCommand(CustomerBL customerRequest)
    {
        CustomerRequest = customerRequest;
    }
}

internal class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly AppDbContext db;

    public UpdateCustomerCommandHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        CustomerDb customerDb = request.CustomerRequest.Adapt<CustomerDb>();
        db.Customers.Update(customerDb);
        int rows = await db.SaveChangesAsync(cancellationToken);
        bool success = rows > 0;
        return success;
    }
}
