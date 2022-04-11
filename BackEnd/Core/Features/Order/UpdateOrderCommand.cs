namespace Core.Handlers.Order;

public class UpdateOrderCommand : IRequest<bool>
{
    public OrderBL OrderRequest { get; }

    public UpdateOrderCommand(OrderBL orderRequest)
    {
        OrderRequest = orderRequest;
    }
}

internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
{
    private readonly AppDbContext db;

    public UpdateOrderCommandHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        OrderDb orderDb = request.Adapt<OrderDb>();
        db.Orders.Update(orderDb);
        int rows = await db.SaveChangesAsync(cancellationToken);
        return rows > 0;
    }
}
