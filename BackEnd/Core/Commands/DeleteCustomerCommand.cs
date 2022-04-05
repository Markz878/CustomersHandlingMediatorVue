using Core.Abstractions;
using Core.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Commands;

public class DeleteCustomerCommand : IRequest<bool>
{
    public int CustomerId { get; }

    public DeleteCustomerCommand(int id)
    {
        CustomerId = id;
    }
}

internal class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly ICustomerRepository customerRepository;
    private readonly IMediator mediator;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IMediator mediator)
    {
        this.customerRepository = customerRepository;
        this.mediator = mediator;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        bool success = await customerRepository.DeleteCustomer(request.CustomerId, cancellationToken);
        if (success)
        {
            await mediator.Publish(new CustomerListChangedNotification(request.CustomerId), cancellationToken);
        }
        return success;
    }
}
