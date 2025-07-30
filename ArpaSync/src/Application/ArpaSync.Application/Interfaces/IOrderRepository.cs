using ArpaSync.Domain.Entities;

namespace ArpaSync.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<List<Order>> GetPaidUnsyncedOrdersAsync();
    Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
    Task<Order> AddAsync(Order order);
    Task<Order> UpdateAsync(Order order);
    Task DeleteAsync(int id);
}