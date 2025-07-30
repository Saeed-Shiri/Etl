using ArpaSync.Application.DTOs.Arpa;
using ArpaSync.Application.Interfaces;
using ArpaSync.Domain.Entities;

namespace ArpaSync.Application.UseCases;

public class SyncOrderWithArpaUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IArpaApiService _arpaApiService;
    private readonly SyncCustomerWithArpaUseCase _syncCustomerUseCase;

    public SyncOrderWithArpaUseCase(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IArpaApiService arpaApiService,
        SyncCustomerWithArpaUseCase syncCustomerUseCase)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _arpaApiService = arpaApiService;
        _syncCustomerUseCase = syncCustomerUseCase;
    }

    public async Task<string> ExecuteAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new ArgumentException($"Order with ID {orderId} not found");

        if (order.Status != OrderStatus.Paid)
            throw new InvalidOperationException("Only paid orders can be synced with Arpa");

        if (!string.IsNullOrEmpty(order.ArpaOrderId))
            return order.ArpaOrderId;

        // اطمینان از سینک شدن مشتری
        var customerBusinessId = await _syncCustomerUseCase.ExecuteAsync(order.CustomerId);

        var arpaOrderDto = new ArpaOrderDto
        {
            OrderNumber = order.OrderNumber,
            CustomerBusinessId = customerBusinessId,
            TotalAmount = order.TotalAmount,
            TaxAmount = order.TaxAmount,
            DiscountAmount = order.DiscountAmount,
            OrderDate = order.OrderDate,
            PaymentDate = order.PaymentDate ?? DateTime.UtcNow,
            Description = order.Description,
            OrderItems = order.OrderItems.Select(item => new ArpaOrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductSku = item.ProductSku,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
                TaxRate = item.TaxRate,
                DiscountAmount = item.DiscountAmount
            }).ToList()
        };

        var arpaOrderId = await _arpaApiService.SendOrderToArpaAsync(arpaOrderDto);

        order.ArpaOrderId = arpaOrderId;
        order.IsSyncedWithArpa = true;
        order.LastSyncDate = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);

        return arpaOrderId;
    }
}