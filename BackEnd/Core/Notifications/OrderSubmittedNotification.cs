namespace Core.Notifications;

public class OrderSubmittedNotification : INotification
{
    public OrderSubmittedNotification(OrderDb order)
    {
        Order = order;
    }
    public OrderDb Order { get; }
}

internal class OrderSubmittedNotificationHandler : INotificationHandler<OrderSubmittedNotification>
{
    private readonly ILogger<OrderSubmittedNotificationHandler> logger;

    public OrderSubmittedNotificationHandler(ILogger<OrderSubmittedNotificationHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(OrderSubmittedNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order with id {orderId} submitted.", notification.Order.Id);
        return Task.CompletedTask;
    }
}