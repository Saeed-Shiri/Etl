namespace ArpaSync.Application.DTOs.Arpa;

public class ArpaCustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? NationalCode { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyRegistrationNumber { get; set; }
    public int CustomerType { get; set; } // 1: Individual, 2: Company
    public ArpaAddressDto Address { get; set; } = new();
}

public class ArpaAddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}