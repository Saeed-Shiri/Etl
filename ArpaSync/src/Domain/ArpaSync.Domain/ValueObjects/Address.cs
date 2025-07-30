namespace ArpaSync.Domain.ValueObjects;

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    
    public Address() { }
    
    public Address(string street, string city, string state, string postalCode, string country)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
    }
    
    public string GetFullAddress()
    {
        return $"{Street}, {City}, {State} {PostalCode}, {Country}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;
        
        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               PostalCode == other.PostalCode &&
               Country == other.Country;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, PostalCode, Country);
    }
}