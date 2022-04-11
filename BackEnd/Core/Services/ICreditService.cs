namespace Core.Services;

public interface ICreditService
{
    Task<double> CheckCustomerCredit(Guid customerId);
}