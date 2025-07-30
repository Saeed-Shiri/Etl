namespace ArpaSync.Application.DTOs.Arpa;

public class ArpaOrderDto
{
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerBusinessId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Description { get; set; }
    public List<ArpaOrderItemDto> OrderItems { get; set; } = new();
}

public class ArpaOrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal DiscountAmount { get; set; }
}