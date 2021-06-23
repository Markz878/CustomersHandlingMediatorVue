using Core.Abstractions;
using Core.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Commands
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public CustomerModel CustomerRequest { get; }

        public UpdateCustomerCommand(CustomerModel customerRequest)
        {
            CustomerRequest = customerRequest;
        }
    }

    internal class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMediator mediator;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMediator mediator)
        {
            this.customerRepository = customerRepository;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            await mediator.Publish(new CustomerListChangedNotification(request.CustomerRequest.Id), cancellationToken);
            return await customerRepository.UpdateCustomer(request.CustomerRequest);
        }
    }
}
