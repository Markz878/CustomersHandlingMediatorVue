namespace Core.Handlers.Order;
public class DeleteOrdersForClientBeforeDateCommand : IRequest<bool>
{
    public DeleteOrdersForClientBeforeDateCommand(Guid clientId, DateTime date)
    {
        ClientId = clientId;
        Date = date;
    }

    public Guid ClientId { get; set; }
    public DateTime Date { get; set; }
}

internal class DeleteOrdersForClientBeforeDateCommandHandler : IRequestHandler<DeleteOrdersForClientBeforeDateCommand, bool>
{
    private readonly AppDbContext db;

    public DeleteOrdersForClientBeforeDateCommandHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<bool> Handle(DeleteOrdersForClientBeforeDateCommand request, CancellationToken cancellationToken)
    {
        List<OrderDb> ordersToDelete = await db.Orders.Where(x=>x.CustomerId == request.ClientId && x.OrderPlacedTime < request.Date).ToListAsync(cancellationToken);
        db.Orders.RemoveRange(ordersToDelete);
        int rows = await db.SaveChangesAsync(cancellationToken);
        return rows > 0;
    }
}
