using ArpaSync.Domain.Entities;

namespace ArpaSync.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByNationalCodeAsync(string nationalCode);
    Task<List<Customer>> GetUnsyncedCustomersAsync();
    Task<Customer> AddAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}