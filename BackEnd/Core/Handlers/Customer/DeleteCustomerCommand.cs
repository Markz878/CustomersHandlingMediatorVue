namespace Core.Handlers.Customer;

public class DeleteCustomerCommand : IRequest<bool>
{
    public Guid CustomerId { get; }
    public DeleteCustomerCommand(Guid id)
    {
        CustomerId = id;
    }
}

internal class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly AppDbContext db;
    private readonly IMediator mediator;

    public DeleteCustomerCommandHandler(AppDbContext db, IMediator mediator)
    {
        this.db = db;
        this.mediator = mediator;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        CustomerDb customer = new() { Id = request.CustomerId };
        db.Customers.Remove(customer);
        int rows = await db.SaveChangesAsync(cancellationToken);
        bool success = rows > 0;
        if (success)
        {
            await mediator.Publish(new CustomerListChangedNotification(request.CustomerId), cancellationToken);
        }
        return success;
    }
}
