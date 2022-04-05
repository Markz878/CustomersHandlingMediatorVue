using Core.Abstractions;
using Core.Models;
using Core.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Commands;

public class AddCustomerCommand : IRequest<int>
{
    public AddCustomerRequest CustomerRequest { get; }

    public AddCustomerCommand(AddCustomerRequest customerRequest)
    {
        CustomerRequest = customerRequest;
    }
}

internal class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, int>
{
    private readonly ICustomerRepository customerRepository;
    private readonly IMediator mediator;

    public AddCustomerCommandHandler(ICustomerRepository customerRepository, IMediator mediator)
    {
        this.customerRepository = customerRepository;
        this.mediator = mediator;
    }

    public async Task<int> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        await mediator.Publish(new CustomerListChangedNotification(null), cancellationToken);
        return await customerRepository.AddCustomer(request.CustomerRequest, cancellationToken);
    }
}
