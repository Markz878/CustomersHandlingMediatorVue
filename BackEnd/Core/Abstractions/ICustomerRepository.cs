using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface ICustomerRepository
    {
        Task<int> AddCustomer(AddCustomerRequest request);
        Task<bool> DeleteCustomer(int id);
        Task<CustomerModel> GetCustomerById(int id);
        Task<IEnumerable<CustomerModel>> GetCustomers();
        Task<bool> UpdateCustomer(CustomerModel customerRequest);
    }
}