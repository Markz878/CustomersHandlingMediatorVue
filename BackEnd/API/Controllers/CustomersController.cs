using Core.Abstractions;
using Core.Commands;
using Core.Models;
using Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomer(int id)
    {
        CustomerModel customer = await mediator.Send(new GetCustomerQuery() { Id = id });
        return customer != null ? Ok(customer) : NotFound("Could not find customer.");
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomerList()
    {
        IEnumerable<CustomerModel> customers = await mediator.Send(new GetCustomerListQuery());
        return customers != null ? Ok(customers) : NotFound("Could not load customers.");
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddCustomer(AddCustomerRequest customerRequest)
    {
        int id = await mediator.Send(new AddCustomerCommand(customerRequest));
        return id > 0 ? Created($"{Request.Scheme}://{Request.Host}{Request.Path}/{id}", null) : BadRequest("Could not add customer.");
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateCustomer(CustomerModel updateCustomerRequest)
    {
        bool success = await mediator.Send(new UpdateCustomerCommand(updateCustomerRequest));
        return success ? Ok() : BadRequest("Could not update customer.");

    }

    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RemoveCustomer(int id)
    {
        bool success = await mediator.Send(new DeleteCustomerCommand(id));
        return success ? Ok() : NotFound("Could not find customer");
    }
}
