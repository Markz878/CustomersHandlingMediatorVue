using Core.Models;

namespace Core.Abstractions;

public class CustomerModel : AddCustomerRequest
{
    public int Id { get; set; }
}
