using Bogus;
using Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    public DbSet<CustomerModel> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CustomerModel>().ToTable("Customers");
    }

    public void SeedData()
    {
        Database.EnsureCreated();
        if (Customers.Any())
        {
            return;
        }
        var customerFaker = new Faker<CustomerModel>()
            .RuleFor(x => x.Id, 0)
            .RuleFor(x => x.FirstName, x => x.Person.FirstName)
            .RuleFor(x => x.LastName, x => x.Person.LastName)
            .RuleFor(x => x.Phone, x => x.Phone.PhoneNumber())
            .RuleFor(x => x.Email, x => x.Person.Email);
        List<CustomerModel> customers = customerFaker.Generate(20);
        Customers.AddRange(customers);
        SaveChanges();
    }
}
