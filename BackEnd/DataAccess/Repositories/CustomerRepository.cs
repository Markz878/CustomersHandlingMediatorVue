using Core.Abstractions;
using Core.Models.Customer;
using DataAccess.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext db;
    private readonly ILogger<CustomerRepository> logger;

    public CustomerRepository(AppDbContext customerDb, ILogger<CustomerRepository> logger)
    {
        db = customerDb;
        this.logger = logger;
    }

    public async Task<CustomerBL?> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        CustomerDb? result = await db.Customers.FindAsync(new object[] { id }, cancellationToken);
        CustomerBL? resultBL = result?.Adapt<CustomerBL>();
        return resultBL;
    }

    public async Task<IEnumerable<CustomerBL>> GetCustomers(CancellationToken cancellationToken)
    {
        List<CustomerDb> customerDbs = await db.Customers.AsNoTracking().ToListAsync(cancellationToken);
        List<CustomerBL> customerBLs = customerDbs.Adapt<List<CustomerBL>>();
        return customerBLs;
    }

    /// <summary>
    /// Add customer to database
    /// </summary>
    /// <returns>Id of the inserted customer</returns>
    /// <exception cref="Exception">Thrown when database insert fails</exception>
    public async Task<Guid> AddCustomer(AddCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            CustomerDb customer = request.Adapt<CustomerDb>();
            db.Customers.Add(customer);
            await db.SaveChangesAsync(cancellationToken);
            return customer.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inserting customer {email}", request.Email);
            throw;
        }
    }

    public async Task<bool> DeleteCustomer(Guid id, CancellationToken cancellationToken)
    {
        CustomerDb customer = new() { Id = id };
        db.Customers.Remove(customer);
        int rows = await db.SaveChangesAsync(cancellationToken);
        return rows > 0;
    }

    public async Task<bool> UpdateCustomer(CustomerBL customerRequest, CancellationToken cancellationToken)
    {
        CustomerDb customerDb = customerRequest.Adapt<CustomerDb>();
        db.Customers.Update(customerDb);
        int rows = await db.SaveChangesAsync(cancellationToken);
        return rows > 0;
    }
}
