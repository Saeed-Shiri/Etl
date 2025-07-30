using ArpaSync.Domain.ValueObjects;

namespace ArpaSync.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? NationalCode { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyRegistrationNumber { get; set; }
    public CustomerType CustomerType { get; set; }
    public Address Address { get; set; } = new();
    
    // Arpa related fields
    public string? ArpaBusinessId { get; set; }
    public bool IsSyncedWithArpa { get; set; }
    public DateTime? LastSyncDate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public enum CustomerType
{
    Individual = 1,
    Company = 2
}