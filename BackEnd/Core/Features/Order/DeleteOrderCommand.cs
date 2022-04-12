namespace Core.Handlers.Order;

public class DeleteOrderCommand : IRequest<bool>
{
    public Guid OrderId { get; }
    public DeleteOrderCommand(Guid id)
    {
        OrderId = id;
    }
}

internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
{
    private readonly AppDbContext db;

    public DeleteOrderCommandHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        OrderDb? order = await db.Orders.FindAsync(new object?[] { request.OrderId }, cancellationToken);
        if (order is null)
        {
            return false;
        }
        db.Orders.Remove(order);
        return true;
    }
}
