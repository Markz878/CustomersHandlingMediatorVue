namespace Core.Notifications;

public class OrderListChangedNotification : INotification
{
    public Guid OrderId { get; }

    public OrderListChangedNotification(Guid orderId)
    {
        OrderId = orderId;
    }
}

internal class OrderListChangedNotificationHandler : INotificationHandler<OrderListChangedNotification>
{
    private readonly IDistributedCache cache;

    public OrderListChangedNotificationHandler(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public async Task Handle(OrderListChangedNotification notification, CancellationToken cancellationToken)
    {
        await cache.RemoveAsync("OrderList", cancellationToken);
        if (notification.OrderId != Guid.Empty)
        {
            await cache.RemoveAsync($"Order-{notification.OrderId}", cancellationToken);
        }
    }
}
