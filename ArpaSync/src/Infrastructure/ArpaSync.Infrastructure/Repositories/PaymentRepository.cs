using ArpaSync.Application.Interfaces;
using ArpaSync.Domain.Entities;
using ArpaSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArpaSync.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Order)
            .Include(p => p.Customer)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Payment>> GetUnsyncedPaymentsAsync()
    {
        return await _context.Payments
            .Include(p => p.Order)
            .Include(p => p.Customer)
            .Where(p => p.Status == PaymentStatus.Completed && 
                       (!p.IsSyncedWithArpa || p.ArpaPaymentId == null))
            .ToListAsync();
    }

    public async Task<List<Payment>> GetPaymentsByOrderIdAsync(int orderId)
    {
        return await _context.Payments
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<List<Payment>> GetPaymentsByCustomerIdAsync(int customerId)
    {
        return await _context.Payments
            .Include(p => p.Order)
            .Where(p => p.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        payment.CreatedAt = DateTime.UtcNow;
        payment.UpdatedAt = DateTime.UtcNow;
        
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        payment.UpdatedAt = DateTime.UtcNow;
        
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}