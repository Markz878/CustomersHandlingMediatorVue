using Core.Abstractions;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext customerDb;
        private readonly ILogger<CustomerRepository> logger;

        public CustomerRepository(CustomerDbContext customerDb, ILogger<CustomerRepository> logger)
        {
            this.customerDb = customerDb;
            TinyMapper.Bind<AddCustomerRequest, CustomerModel>();
            this.logger = logger;
        }

        public async Task<CustomerModel> GetCustomerById(int id)
        {
            var result = await customerDb.Customers.FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            var result = await customerDb.Customers.AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<int> AddCustomer(AddCustomerRequest request)
        {
            try
            {
                CustomerModel customer = TinyMapper.Map<CustomerModel>(request);
                customerDb.Customers.Add(customer);
                await customerDb.SaveChangesAsync();
                return customer.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inserting customer {0}", request.Email);
                return -1;
            }

        }

        public async Task<bool> DeleteCustomer(int id)
        {
            CustomerModel customer = new() { Id = id };
            customerDb.Customers.Remove(customer);
            int rows = await customerDb.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> UpdateCustomer(CustomerModel customerRequest)
        {
            customerDb.Update(customerRequest);
            int rows = await customerDb.SaveChangesAsync();
            return rows > 0;
        }
    }
}
