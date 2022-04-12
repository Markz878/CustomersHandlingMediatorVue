namespace Core.Handlers.Order;

public class GetOrdersForCustomerQuery : IRequest<IEnumerable<OrderBL>>, ICacheableQuery
{
    public Guid CustomerId { get; init; }
    public bool BypassCache { get; init; }
    public string Cachekey => $"orders-for-{CustomerId}";
    public TimeSpan? SlidingExpiration { get; init; }
}

internal class GetOrdersForCustomerQueryHandler : IRequestHandler<GetOrdersForCustomerQuery, IEnumerable<OrderBL>>
{
    private readonly AppDbContext db;

    public GetOrdersForCustomerQueryHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<IEnumerable<OrderBL>> Handle(GetOrdersForCustomerQuery request, CancellationToken cancellationToken)
    {
        List<OrderDb> orders = await db.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(x => x.CustomerId == request.CustomerId)
            .ToListAsync(cancellationToken);
        List<OrderBL> orderBLs = orders.Adapt<List<OrderBL>>();
        return orderBLs;
    }
}
