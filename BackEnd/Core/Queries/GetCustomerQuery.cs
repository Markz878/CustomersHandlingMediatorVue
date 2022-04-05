using Core.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Queries;

public class GetCustomerQuery : IRequest<CustomerModel>, ICacheableQuery
{
    public int Id { get; init; }
    public bool BypassCache { get; init; }
    public string Cachekey => $"Customer-{Id}";
    public TimeSpan? SlidingExpiration { get; init; }
}

internal class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerModel>
{
    private readonly ICustomerRepository customerRepository;

    public GetCustomerQueryHandler(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public Task<CustomerModel> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        return customerRepository.GetCustomerById(request.Id, cancellationToken);
    }
}
