namespace Core.Handlers.Order;

public class SubmitOrderCommand : IRequest<SubmitOrderResponse>
{
    public Guid OrderId { get; }
    public SubmitOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}

public enum SubmitOrderResponse
{
    NotFound,
    OrderAlreadySubmitted,
    CreditExceeded,
    Success
}

internal class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, SubmitOrderResponse>
{
    private readonly AppDbContext db;
    private readonly IMediator mediator;
    private readonly ICreditService creditService;

    public SubmitOrderCommandHandler(AppDbContext db, IMediator mediator, ICreditService creditService)
    {
        this.db = db;
        this.mediator = mediator;
        this.creditService = creditService;
    }

    public async Task<SubmitOrderResponse> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        OrderDb? order = await db.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
        if (order == null)
        {
            return SubmitOrderResponse.NotFound;
        }
        if (order.State > 0)
        {
            return SubmitOrderResponse.OrderAlreadySubmitted;
        }
        double customerCredit = await creditService.CheckCustomerCredit(order.CustomerId);
        if (order.Items.Sum(x => x.Amount * x.Price) > (decimal)customerCredit)
        {
            return SubmitOrderResponse.CreditExceeded;
        }
        order.State = OrderState.Ordered;
        await db.SaveChangesAsync(cancellationToken);
        await mediator.Publish(new OrderSubmittedNotification(order), cancellationToken);
        return SubmitOrderResponse.Success;
    }
}
