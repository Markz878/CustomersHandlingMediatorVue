using Core.Features.Customer;
using Core.Handlers.Customer;
using Core.Models.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator mediator;

    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerWithourOrdersDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        CustomerWithourOrdersDto? customer = await mediator.Send(new GetCustomerQuery(id), cancellationToken);
        return customer != null ? Ok(customer) : NotFound("Could not find customer.");
    }

    [HttpGet("with-orders/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerBL))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetCustomerWithOrders(Guid id, bool splitQuery, CancellationToken cancellationToken)
    {
        CustomerBL? customer = await mediator.Send(new GetCustomerWithOrdersQuery(id, splitQuery), cancellationToken);
        return customer != null ? Ok(customer) : NotFound("Could not find customer.");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerWithourOrdersDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetCustomerList(CancellationToken cancellationToken)
    {
        IEnumerable<CustomerWithourOrdersDto> customers = await mediator.Send(new GetCustomerListQuery(), cancellationToken);
        return customers != null ? Ok(customers) : NotFound("Could not load customers.");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> AddCustomer(AddCustomerRequest customerRequest)
    {
        Guid id = await mediator.Send(new AddCustomerCommand(customerRequest));
        return id != Guid.Empty ? Created($"{Request.Scheme}://{Request.Host}{Request.Path}/{id}", null) : BadRequest("Could not add customer.");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> UpdateCustomer(CustomerBL updateCustomerRequest)
    {
        bool success = await mediator.Send(new UpdateCustomerCommand(updateCustomerRequest));
        return success ? NoContent() : BadRequest("Could not update customer.");
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> RemoveCustomer(Guid id)
    {
        bool success = await mediator.Send(new DeleteCustomerCommand(id));
        return success ? NoContent() : NotFound("Could not find customer");
    }
}
