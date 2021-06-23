using Core.Abstractions;
using Core.Models;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfraStructure
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Task<CustomerModel> GetCustomer(int id)
        {
            return customerRepository.GetCustomerById(id);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomerList()
        {
            await Task.Delay(1000);
            return await customerRepository.GetCustomers();
        }

        public async Task<int> AddCustomer(AddCustomerRequest request)
        {
            await Task.Delay(500);
            try
            {
                return await customerRepository.AddCustomer(request);
            }
            catch (System.Exception)
            {
                return -1;
            }
        }

        public Task<bool> DeleteCustomer(int id)
        {
            return customerRepository.DeleteCustomer(id);
        }
    }
}
