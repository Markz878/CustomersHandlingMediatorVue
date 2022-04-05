using Core.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Queries;

public class GetCustomerListQuery : IRequest<IEnumerable<CustomerModel>>, ICacheableQuery
{
    public bool BypassCache { get; init; }
    public string Cachekey => "CustomerList";
    public TimeSpan? SlidingExpiration { get; init; }
}

internal class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, IEnumerable<CustomerModel>>
{
    private readonly ICustomerRepository customerRepository;

    public GetCustomerListQueryHandler(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public Task<IEnumerable<CustomerModel>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        return customerRepository.GetCustomers();
    }
}
