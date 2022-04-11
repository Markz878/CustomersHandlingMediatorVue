namespace Core.Handlers.Customer;

public class GetCustomerWithOrdersQuery : IRequest<CustomerBL?>
{
    public GetCustomerWithOrdersQuery(Guid id, bool asSplitQuery)
    {
        Id = id;
        AsSplitQuery = asSplitQuery;
    }
    public Guid Id { get; init; }
    public bool AsSplitQuery { get; init; }
}

internal class GetCustomerWithOrdersQueryHandler : IRequestHandler<GetCustomerWithOrdersQuery, CustomerBL?>
{
    private readonly AppDbContext db;

    public GetCustomerWithOrdersQueryHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<CustomerBL?> Handle(GetCustomerWithOrdersQuery request, CancellationToken cancellationToken)
    {
        CustomerDb? result;
        if (request.AsSplitQuery)
        {
            result = await db.Customers.AsNoTracking().Include(x => x.Orders).ThenInclude(x => x.Items).AsSplitQuery().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
        else
        {
            result = await db.Customers.AsNoTracking().Include(x => x.Orders).ThenInclude(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
        CustomerBL? resultBL = result?.Adapt<CustomerBL>();
        return resultBL;
    }
}
