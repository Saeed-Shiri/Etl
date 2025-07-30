namespace ArpaSync.Application.DTOs.Arpa;

public class ArpaPaymentDto
{
    public string CustomerBusinessId { get; set; } = string.Empty;
    public string? OrderId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Description { get; set; }
}