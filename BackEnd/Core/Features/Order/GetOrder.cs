namespace Core.Handlers.Order;

public class GetOrderQuery : IRequest<OrderBL?>, ICacheableQuery
{
    public Guid OrderId { get; init; }
    public bool BypassCache { get; init; }
    public string Cachekey => $"order-{OrderId}";
    public TimeSpan? SlidingExpiration { get; init; }
}

internal class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderBL?>
{
    private readonly AppDbContext db;

    public GetOrderQueryHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<OrderBL?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        OrderDb? orderDb = await db.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);
        OrderBL? orderBL = orderDb?.Adapt<OrderBL>();
        return orderBL;
    }
}
