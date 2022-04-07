using Bogus;
using Core.Models.Order;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    internal DbSet<CustomerDb> Customers => Set<CustomerDb>();
    internal DbSet<OrderDb> Orders => Set<OrderDb>();
    internal DbSet<OrderItemDb> OrderItems => Set<OrderItemDb>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CustomerDb>().ToTable("Customers");
        builder.Entity<OrderDb>().ToTable("Orders");
        builder.Entity<OrderItemDb>().ToTable("OrderItems");
    }

    public void SeedData()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
        if (Customers.Any())
        {
            return;
        }
        Faker<CustomerDb> customerFaker = new Faker<CustomerDb>()
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Email, f => f.Person.Email);
        Faker<OrderDb> orderFaker = new Faker<OrderDb>()
            .RuleFor(x => x.State, OrderState.Ordered)
            .RuleFor(x => x.OrderPlacedTime, f => f.Date.PastOffset());
        Faker<OrderItemDb> itemFaker = new Faker<OrderItemDb>()
            .RuleFor(x => x.Name, f => f.Commerce.Product())
            .RuleFor(x => x.Price, f => f.Random.Decimal(1, 100))
            .RuleFor(x => x.Amount, f => f.Random.Int(1, 5));
        List<CustomerDb> customers = customerFaker.Generate(20);
        foreach (var c in customers)
        {
            c.Orders = orderFaker.GenerateBetween(1,10);
            foreach (var o in c.Orders)
            {
                o.Items = itemFaker.GenerateBetween(1,10);
            }
        }
        Customers.AddRange(customers);
        SaveChanges();
    }
}
