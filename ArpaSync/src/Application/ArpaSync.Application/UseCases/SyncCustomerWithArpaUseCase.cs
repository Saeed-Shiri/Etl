using ArpaSync.Application.DTOs.Arpa;
using ArpaSync.Application.Interfaces;
using ArpaSync.Domain.Entities;

namespace ArpaSync.Application.UseCases;

public class SyncCustomerWithArpaUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IArpaApiService _arpaApiService;

    public SyncCustomerWithArpaUseCase(
        ICustomerRepository customerRepository,
        IArpaApiService arpaApiService)
    {
        _customerRepository = customerRepository;
        _arpaApiService = arpaApiService;
    }

    public async Task<string> ExecuteAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            throw new ArgumentException($"Customer with ID {customerId} not found");

        if (!string.IsNullOrEmpty(customer.ArpaBusinessId))
            return customer.ArpaBusinessId;

        var arpaCustomerDto = new ArpaCustomerDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            NationalCode = customer.NationalCode,
            CompanyName = customer.CompanyName,
            CompanyRegistrationNumber = customer.CompanyRegistrationNumber,
            CustomerType = (int)customer.CustomerType,
            Address = new ArpaAddressDto
            {
                Street = customer.Address.Street,
                City = customer.Address.City,
                State = customer.Address.State,
                PostalCode = customer.Address.PostalCode,
                Country = customer.Address.Country
            }
        };

        var businessId = await _arpaApiService.EnsureCustomerExistsAsync(arpaCustomerDto);

        customer.ArpaBusinessId = businessId;
        customer.IsSyncedWithArpa = true;
        customer.LastSyncDate = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(customer);

        return businessId;
    }
}