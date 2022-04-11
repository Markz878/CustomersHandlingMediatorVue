namespace Core.Handlers.Customer;

public class GetCustomerListQuery : IRequest<IEnumerable<CustomerWithourOrdersDto>>, ICacheableQuery
{
    public bool BypassCache { get; init; }
    public string Cachekey => "customer-list";
    public TimeSpan? SlidingExpiration { get; init; }
}

internal class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, IEnumerable<CustomerWithourOrdersDto>>
{
    private readonly AppDbContext db;

    public GetCustomerListQueryHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<IEnumerable<CustomerWithourOrdersDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        List<CustomerDb> customerDbs = await db.Customers.AsNoTracking().ToListAsync(cancellationToken);
        List<CustomerWithourOrdersDto> customerBLs = customerDbs.Adapt<List<CustomerWithourOrdersDto>>();
        return customerBLs;
    }
}
