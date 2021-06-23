using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Notifications
{
    public class CustomerListChangedNotification : INotification
    {
        public int? CustomerId { get; }

        public CustomerListChangedNotification(int? customerId)
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
            if (notification.CustomerId != null)
            {
                await cache.RemoveAsync($"Customer-{notification.CustomerId}", cancellationToken);
            }
        }
    }
}
