namespace Core.Handlers.Customer;

public class GetCustomerQuery : IRequest<CustomerBL?>, ICacheableQuery
{
    public GetCustomerQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; init; }
    public bool BypassCache { get; init; }
    public string Cachekey => $"customer-{Id}";
    public TimeSpan? SlidingExpiration { get; init; }
}

internal class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerBL?>
{
    private readonly AppDbContext db;

    public GetCustomerQueryHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<CustomerBL?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        CustomerDb? result = await db.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
        CustomerBL? resultBL = result?.Adapt<CustomerBL>();
        return resultBL;
    }
}
