namespace Core.Handlers.Customer;

public class GetCustomerListQuery : IRequest<IEnumerable<CustomerBL>>, ICacheableQuery
{
    public bool BypassCache { get; init; }
    public string Cachekey => "customer-list";
    public TimeSpan? SlidingExpiration { get; init; }
}

internal class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, IEnumerable<CustomerBL>>
{
    private readonly AppDbContext db;

    public GetCustomerListQueryHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<IEnumerable<CustomerBL>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        List<CustomerDb> customerDbs = await db.Customers.AsNoTracking().ToListAsync(cancellationToken);
        List<CustomerBL> customerBLs = customerDbs.Adapt<List<CustomerBL>>();
        return customerBLs;
    }
}
