using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class CustomerDb
{
    public Guid Id { get; set; }
    [Required]
    [MinLength(3)]
    public string? FirstName { get; set; }
    [Required]
    [MinLength(3)]
    public string? LastName { get; set; }
    [Required]
    [MinLength(8)]
    public string? Phone { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    public ICollection<OrderDb> Orders { get; set; } = new List<OrderDb>();
}
