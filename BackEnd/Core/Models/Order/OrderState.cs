namespace Core.Models.Order;

public enum OrderState
{
    Draft,
    Ordered,
    Fulfilled,
    Shipped,
    Received
}