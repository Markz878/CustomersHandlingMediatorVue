namespace Core.Data.Models;
#nullable disable
public class CustomerDb
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public List<OrderDb> Orders { get; set; } = new List<OrderDb>();
}
