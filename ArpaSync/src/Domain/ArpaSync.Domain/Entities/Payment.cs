namespace ArpaSync.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionId { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? GatewayResponse { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Description { get; set; }
    
    // Arpa related fields
    public bool IsSyncedWithArpa { get; set; }
    public DateTime? LastSyncDate { get; set; }
    public string? ArpaPaymentId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public Order Order { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}

public enum PaymentMethod
{
    CreditCard = 1,
    BankTransfer = 2,
    Cash = 3,
    OnlinePayment = 4,
    Wallet = 5
}

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Cancelled = 4,
    Refunded = 5
}