using Core.Abstractions;
using Core.Models.Order;
using DataAccess.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext db;
    private readonly ILogger<OrderRepository> logger;

    public OrderRepository(AppDbContext customerDb, ILogger<OrderRepository> logger)
    {
        db = customerDb;
        this.logger = logger;
    }

    public async Task<Guid> AddOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            OrderDb order = request.Adapt<OrderDb>();
            db.Orders.Add(order);
            await db.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inserting order for customer id {customerId}", request.CustomerId);
            return Guid.Empty;
        }
    }

    public async Task<bool> DeleteOrder(Guid id, CancellationToken cancellationToken)
    {
        OrderDb? order = await db.Orders.FindAsync(new object?[] { id }, cancellationToken);
        if (order is null)
        {
            return false;
        }
        db.Orders.Remove(order);
        return true;
    }

    public async Task<OrderBL?> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        OrderDb? orderDb = await db.Orders.AsNoTracking()
            .Include(x=>x.Items)
            .FirstOrDefaultAsync(x=> x.Id ==  id, cancellationToken);
        OrderBL? orderBL = orderDb?.Adapt<OrderBL>();
        return orderBL;
    }

    public async Task<IEnumerable<OrderBL>> GetOrdersForCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        List<OrderDb> orders = await db.Orders
            .AsNoTracking()
            .Include(x=>x.Items)
            .Where(x => x.CustomerId == customerId)
            .ToListAsync(cancellationToken);
        List<OrderBL> orderBLs = orders.Adapt<List<OrderBL>>();
        return orderBLs;
    }

    public async Task<bool> UpdateOrder(OrderBL request, CancellationToken cancellationToken)
    {
        try
        {
            OrderDb orderDb = request.Adapt<OrderDb>();
            db.Orders.Update(orderDb);
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not update order with id {orderId}", request.Id);
            return false;
        }
    }
}
