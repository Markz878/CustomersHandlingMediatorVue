namespace Core.Services;

public class CreditService : ICreditService
{
    public Task<double> CheckCustomerCredit(Guid customerId)
    {
        return Task.FromResult(Random.Shared.NextDouble() * 1000 + 500);
    }
}
