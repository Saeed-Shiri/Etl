namespace ArpaSync.Domain.Events;

public class CustomerCreatedEvent
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? NationalCode { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyRegistrationNumber { get; set; }
    public int CustomerType { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public CustomerCreatedEvent(int customerId, string firstName, string lastName, 
        string email, string phoneNumber, string? nationalCode, string? companyName, 
        string? companyRegistrationNumber, int customerType, DateTime createdAt)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        NationalCode = nationalCode;
        CompanyName = companyName;
        CompanyRegistrationNumber = companyRegistrationNumber;
        CustomerType = customerType;
        CreatedAt = createdAt;
    }
}