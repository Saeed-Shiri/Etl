namespace ArpaSync.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? Description { get; set; }
    
    // Arpa related fields
    public bool IsSyncedWithArpa { get; set; }
    public DateTime? LastSyncDate { get; set; }
    public string? ArpaOrderId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public enum OrderStatus
{
    Pending = 1,
    Confirmed = 2,
    Paid = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6
}