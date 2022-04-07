namespace Core.Handlers.Customer;

public class AddCustomerCommand : IRequest<Guid>
{
    public AddCustomerRequest CustomerRequest { get; }
    public AddCustomerCommand(AddCustomerRequest customerRequest)
    {
        CustomerRequest = customerRequest;
    }
}

internal class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, Guid>
{
    private readonly AppDbContext db;
    private readonly IMediator mediator;

    public AddCustomerCommandHandler(AppDbContext db, IMediator mediator)
    {
        this.db = db;
        this.mediator = mediator;
    }

    public async Task<Guid> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        CustomerDb customer = request.CustomerRequest.Adapt<CustomerDb>();
        db.Customers.Add(customer);
        await db.SaveChangesAsync(cancellationToken);
        await mediator.Publish(new CustomerListChangedNotification(null), cancellationToken);
        return customer.Id;
    }
}
