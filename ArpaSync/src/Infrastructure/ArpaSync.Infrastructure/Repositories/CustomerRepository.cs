using ArpaSync.Application.Interfaces;
using ArpaSync.Domain.Entities;
using ArpaSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArpaSync.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer?> GetByNationalCodeAsync(string nationalCode)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.NationalCode == nationalCode);
    }

    public async Task<List<Customer>> GetUnsyncedCustomersAsync()
    {
        return await _context.Customers
            .Where(c => !c.IsSyncedWithArpa || c.ArpaBusinessId == null)
            .ToListAsync();
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}