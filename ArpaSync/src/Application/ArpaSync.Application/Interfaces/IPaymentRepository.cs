using ArpaSync.Domain.Entities;

namespace ArpaSync.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task<List<Payment>> GetUnsyncedPaymentsAsync();
    Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId);
    Task<List<Payment>> GetPaymentsByCustomerIdAsync(int customerId);
    Task<Payment> AddAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task DeleteAsync(int id);
}