using Core.Handlers.Order;
using Core.Models.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator mediator;

    public OrdersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderBL))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetOrders(Guid id)
    {
        OrderBL? order = await mediator.Send(new GetOrderQuery() { OrderId = id });
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderBL>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetCustomerOrders(Guid customerId)
    {
        IEnumerable<OrderBL> orders = await mediator.Send(new GetOrdersForCustomerQuery() { CustomerId = customerId });
        return Ok(orders);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> AddOrder(CreateOrderRequest orderRequest)
    {
        Guid id = await mediator.Send(new CreateOrderCommand(orderRequest));
        return id != Guid.Empty ? Created($"{Request.Scheme}://{Request.Host}{Request.Path}/{id}", orderRequest) : BadRequest("Could not create order.");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateOrder(OrderBL updateOrderRequest)
    {
        bool success = await mediator.Send(new UpdateOrderCommand(updateOrderRequest));
        return success ? NoContent() : BadRequest("Could not update order.");

    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        bool success = await mediator.Send(new DeleteOrderCommand(id));
        return success ? NoContent() : NotFound("Could not find order.");
    }

    [HttpDelete("beforedate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> DeleteOrdersForClientBeforeDate(Guid customerId, DateTime date)
    {
        bool success = await mediator.Send(new DeleteOrdersForClientBeforeDateCommand(customerId, date));
        return success ? NoContent() : NotFound("Could not find order.");
    }

    [HttpPatch("addtomatoes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> AddTomatoesToEveryThirdOrderForClient(Guid customerId)
    {
        bool success = await mediator.Send(new AddTomatoesToEveryThirdOrderForCustomerCommand(customerId));
        return success ? NoContent() : BadRequest("Could not add tomatoes.");
    }
}
