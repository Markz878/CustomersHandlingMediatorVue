namespace Core.Handlers.Order;

public class CreateOrderCommand : IRequest<Guid>
{
    public CreateOrderRequest OrderRequest { get; }

    public CreateOrderCommand(CreateOrderRequest orderRequest)
    {
        OrderRequest = orderRequest;
    }
}

internal class AddOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly AppDbContext db;

    public AddOrderCommandHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        OrderDb order = request.Adapt<OrderDb>();
        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}
