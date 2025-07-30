using ArpaSync.Application.DTOs.Arpa;
using ArpaSync.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace ArpaSync.Infrastructure.Services;

public class ArpaApiService : IArpaApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ArpaApiService> _logger;
    private readonly string _baseUrl;
    private readonly string _apiKey;

    public ArpaApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ArpaApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _baseUrl = _configuration["ArpaApi:BaseUrl"] ?? throw new InvalidOperationException("ArpaApi:BaseUrl not configured");
        _apiKey = _configuration["ArpaApi:ApiKey"] ?? throw new InvalidOperationException("ArpaApi:ApiKey not configured");
        
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<string> EnsureCustomerExistsAsync(ArpaCustomerDto customerData)
    {
        try
        {
            _logger.LogInformation("Checking/Creating customer in Arpa: {Email}", customerData.Email);

            // First, try to find existing customer
            var existingCustomer = await FindCustomerAsync(customerData);
            if (existingCustomer != null)
            {
                _logger.LogInformation("Customer found in Arpa with BusinessId: {BusinessId}", existingCustomer.BusinessId);
                return existingCustomer.BusinessId;
            }

            // Create new customer
            var createCustomerRequest = new
            {
                FirstName = customerData.FirstName,
                LastName = customerData.LastName,
                Email = customerData.Email,
                PhoneNumber = customerData.PhoneNumber,
                NationalCode = customerData.NationalCode,
                CompanyName = customerData.CompanyName,
                CompanyRegistrationNumber = customerData.CompanyRegistrationNumber,
                CustomerType = customerData.CustomerType,
                Address = new
                {
                    Street = customerData.Address.Street,
                    City = customerData.Address.City,
                    State = customerData.Address.State,
                    PostalCode = customerData.Address.PostalCode,
                    Country = customerData.Address.Country
                }
            };

            var json = JsonSerializer.Serialize(createCustomerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/customers", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var createResult = JsonSerializer.Deserialize<ArpaCustomerResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (createResult == null || string.IsNullOrEmpty(createResult.BusinessId))
                throw new InvalidOperationException("Failed to create customer in Arpa - no BusinessId returned");

            _logger.LogInformation("Customer created in Arpa with BusinessId: {BusinessId}", createResult.BusinessId);
            return createResult.BusinessId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ensuring customer exists in Arpa: {Email}", customerData.Email);
            throw;
        }
    }

    public async Task<string> SendOrderToArpaAsync(ArpaOrderDto orderData)
    {
        try
        {
            _logger.LogInformation("Sending order to Arpa: {OrderNumber}", orderData.OrderNumber);

            var sendOrderRequest = new
            {
                OrderNumber = orderData.OrderNumber,
                CustomerBusinessId = orderData.CustomerBusinessId,
                TotalAmount = orderData.TotalAmount,
                TaxAmount = orderData.TaxAmount,
                DiscountAmount = orderData.DiscountAmount,
                OrderDate = orderData.OrderDate,
                PaymentDate = orderData.PaymentDate,
                Description = orderData.Description,
                OrderItems = orderData.OrderItems.Select(item => new
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

            var json = JsonSerializer.Serialize(sendOrderRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/orders", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ArpaOrderResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null || string.IsNullOrEmpty(result.OrderId))
                throw new InvalidOperationException("Failed to send order to Arpa - no OrderId returned");

            _logger.LogInformation("Order sent to Arpa with OrderId: {OrderId}", result.OrderId);
            return result.OrderId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending order to Arpa: {OrderNumber}", orderData.OrderNumber);
            throw;
        }
    }

    public async Task<string> SendPaymentToArpaAsync(ArpaPaymentDto paymentData)
    {
        try
        {
            _logger.LogInformation("Sending payment to Arpa for customer: {CustomerBusinessId}", paymentData.CustomerBusinessId);

            var sendPaymentRequest = new
            {
                CustomerBusinessId = paymentData.CustomerBusinessId,
                OrderId = paymentData.OrderId,
                Amount = paymentData.Amount,
                PaymentMethod = paymentData.PaymentMethod,
                TransactionId = paymentData.TransactionId,
                ReferenceNumber = paymentData.ReferenceNumber,
                PaymentDate = paymentData.PaymentDate,
                Description = paymentData.Description
            };

            var json = JsonSerializer.Serialize(sendPaymentRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/payments", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ArpaPaymentResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null || string.IsNullOrEmpty(result.PaymentId))
                throw new InvalidOperationException("Failed to send payment to Arpa - no PaymentId returned");

            _logger.LogInformation("Payment sent to Arpa with PaymentId: {PaymentId}", result.PaymentId);
            return result.PaymentId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending payment to Arpa for customer: {CustomerBusinessId}", paymentData.CustomerBusinessId);
            throw;
        }
    }

    private async Task<ArpaCustomerResponse?> FindCustomerAsync(ArpaCustomerDto customerData)
    {
        try
        {
            // Try to find by email first
            var response = await _httpClient.GetAsync($"/api/customers/search?email={Uri.EscapeDataString(customerData.Email)}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ArpaCustomerResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }

            // If not found by email and has national code, try national code
            if (!string.IsNullOrEmpty(customerData.NationalCode))
            {
                response = await _httpClient.GetAsync($"/api/customers/search?nationalCode={Uri.EscapeDataString(customerData.NationalCode)}");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ArpaCustomerResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error searching for customer in Arpa: {Email}", customerData.Email);
            return null;
        }
    }
}

// Response DTOs for Arpa API
public class ArpaCustomerResponse
{
    public string BusinessId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ArpaOrderResponse
{
    public string OrderId { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ArpaPaymentResponse
{
    public string PaymentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}