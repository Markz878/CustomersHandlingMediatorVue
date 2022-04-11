namespace Core.Handlers.Customer;

public class GetCustomerQuery : IRequest<CustomerWithourOrdersDto?>
{
    public GetCustomerQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; init; }
}

internal class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerWithourOrdersDto?>
{
    private readonly AppDbContext db;

    public GetCustomerQueryHandler(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<CustomerWithourOrdersDto?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        CustomerDb? result = await db.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
        CustomerWithourOrdersDto? resultBL = result?.Adapt<CustomerWithourOrdersDto>();
        return resultBL;
    }
}
