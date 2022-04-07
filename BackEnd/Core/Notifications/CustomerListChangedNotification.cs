namespace Core.Notifications;

public class CustomerListChangedNotification : INotification
{
    public Guid? CustomerId { get; }

    public CustomerListChangedNotification(Guid? customerId)
    {
        CustomerId = customerId;
    }
}

internal class CustomerListChangedNotificationHandler : INotificationHandler<CustomerListChangedNotification>
{
    private readonly IDistributedCache cache;

    public CustomerListChangedNotificationHandler(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public async Task Handle(CustomerListChangedNotification notification, CancellationToken cancellationToken)
    {
        await cache.RemoveAsync("CustomerList", cancellationToken);
        if (notification.CustomerId != Guid.Empty)
        {
            await cache.RemoveAsync($"Customer-{notification.CustomerId}", cancellationToken);
        }
    }
}
