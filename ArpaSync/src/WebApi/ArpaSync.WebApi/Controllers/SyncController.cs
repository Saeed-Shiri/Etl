using ArpaSync.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ArpaSync.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly SyncCustomerWithArpaUseCase _syncCustomerUseCase;
    private readonly SyncOrderWithArpaUseCase _syncOrderUseCase;
    private readonly SyncPaymentWithArpaUseCase _syncPaymentUseCase;
    private readonly ILogger<SyncController> _logger;

    public SyncController(
        SyncCustomerWithArpaUseCase syncCustomerUseCase,
        SyncOrderWithArpaUseCase syncOrderUseCase,
        SyncPaymentWithArpaUseCase syncPaymentUseCase,
        ILogger<SyncController> logger)
    {
        _syncCustomerUseCase = syncCustomerUseCase;
        _syncOrderUseCase = syncOrderUseCase;
        _syncPaymentUseCase = syncPaymentUseCase;
        _logger = logger;
    }

    /// <summary>
    /// Manually sync a specific customer with Arpa
    /// </summary>
    [HttpPost("customer/{customerId}")]
    public async Task<IActionResult> SyncCustomer(int customerId)
    {
        try
        {
            _logger.LogInformation("Manual sync requested for customer: {CustomerId}", customerId);
            
            var businessId = await _syncCustomerUseCase.ExecuteAsync(customerId);
            
            return Ok(new { 
                Success = true, 
                Message = "Customer synced successfully", 
                BusinessId = businessId 
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Customer not found: {CustomerId}", customerId);
            return NotFound(new { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing customer: {CustomerId}", customerId);
            return StatusCode(500, new { Success = false, Message = "Internal server error" });
        }
    }

    /// <summary>
    /// Manually sync a specific order with Arpa
    /// </summary>
    [HttpPost("order/{orderId}")]
    public async Task<IActionResult> SyncOrder(int orderId)
    {
        try
        {
            _logger.LogInformation("Manual sync requested for order: {OrderId}", orderId);
            
            var arpaOrderId = await _syncOrderUseCase.ExecuteAsync(orderId);
            
            return Ok(new { 
                Success = true, 
                Message = "Order synced successfully", 
                ArpaOrderId = arpaOrderId 
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Order not found: {OrderId}", orderId);
            return NotFound(new { Success = false, Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation for order: {OrderId}", orderId);
            return BadRequest(new { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing order: {OrderId}", orderId);
            return StatusCode(500, new { Success = false, Message = "Internal server error" });
        }
    }

    /// <summary>
    /// Manually sync a specific payment with Arpa
    /// </summary>
    [HttpPost("payment/{paymentId}")]
    public async Task<IActionResult> SyncPayment(int paymentId)
    {
        try
        {
            _logger.LogInformation("Manual sync requested for payment: {PaymentId}", paymentId);
            
            var arpaPaymentId = await _syncPaymentUseCase.ExecuteAsync(paymentId);
            
            return Ok(new { 
                Success = true, 
                Message = "Payment synced successfully", 
                ArpaPaymentId = arpaPaymentId 
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Payment not found: {PaymentId}", paymentId);
            return NotFound(new { Success = false, Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation for payment: {PaymentId}", paymentId);
            return BadRequest(new { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing payment: {PaymentId}", paymentId);
            return StatusCode(500, new { Success = false, Message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get sync status and statistics
    /// </summary>
    [HttpGet("status")]
    public IActionResult GetSyncStatus()
    {
        try
        {
            // This would typically fetch statistics from repositories
            // For now, returning a simple status
            return Ok(new
            {
                Success = true,
                Status = "Running",
                Message = "Sync service is operational",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sync status");
            return StatusCode(500, new { Success = false, Message = "Internal server error" });
        }
    }
}