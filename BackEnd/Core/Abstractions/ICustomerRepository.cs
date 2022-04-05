using Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions;

public interface ICustomerRepository
{
    Task<int> AddCustomer(AddCustomerRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteCustomer(int id, CancellationToken cancellationToken);
    Task<CustomerModel> GetCustomerById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<CustomerModel>> GetCustomers(CancellationToken cancellationToken);
    Task<bool> UpdateCustomer(CustomerModel customerRequest, CancellationToken cancellationToken);
}
