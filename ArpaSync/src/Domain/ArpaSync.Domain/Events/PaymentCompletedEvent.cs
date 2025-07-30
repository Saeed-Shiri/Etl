namespace ArpaSync.Domain.Events;

public class PaymentCompletedEvent
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Description { get; set; }
    
    public PaymentCompletedEvent(int paymentId, int orderId, int customerId, 
        decimal amount, int paymentMethod, string? transactionId, 
        string? referenceNumber, DateTime paymentDate, string? description)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        CustomerId = customerId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        TransactionId = transactionId;
        ReferenceNumber = referenceNumber;
        PaymentDate = paymentDate;
        Description = description;
    }
}