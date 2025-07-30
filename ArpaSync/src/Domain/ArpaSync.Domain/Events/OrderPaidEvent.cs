namespace ArpaSync.Domain.Events;

public class OrderPaidEvent
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public List<OrderItemInfo> OrderItems { get; set; } = new();
    
    public OrderPaidEvent(int orderId, string orderNumber, int customerId, 
        decimal totalAmount, decimal taxAmount, decimal discountAmount, 
        DateTime orderDate, DateTime paymentDate, List<OrderItemInfo> orderItems)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        TaxAmount = taxAmount;
        DiscountAmount = discountAmount;
        OrderDate = orderDate;
        PaymentDate = paymentDate;
        OrderItems = orderItems;
    }
}

public class OrderItemInfo
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