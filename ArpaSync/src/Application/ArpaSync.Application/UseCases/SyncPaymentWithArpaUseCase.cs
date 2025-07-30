using ArpaSync.Application.DTOs.Arpa;
using ArpaSync.Application.Interfaces;
using ArpaSync.Domain.Entities;

namespace ArpaSync.Application.UseCases;

public class SyncPaymentWithArpaUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IArpaApiService _arpaApiService;
    private readonly SyncCustomerWithArpaUseCase _syncCustomerUseCase;
    private readonly SyncOrderWithArpaUseCase _syncOrderUseCase;

    public SyncPaymentWithArpaUseCase(
        IPaymentRepository paymentRepository,
        ICustomerRepository customerRepository,
        IArpaApiService arpaApiService,
        SyncCustomerWithArpaUseCase syncCustomerUseCase,
        SyncOrderWithArpaUseCase syncOrderUseCase)
    {
        _paymentRepository = paymentRepository;
        _customerRepository = customerRepository;
        _arpaApiService = arpaApiService;
        _syncCustomerUseCase = syncCustomerUseCase;
        _syncOrderUseCase = syncOrderUseCase;
    }

    public async Task<string> ExecuteAsync(int paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
            throw new ArgumentException($"Payment with ID {paymentId} not found");

        if (payment.Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Only completed payments can be synced with Arpa");

        if (!string.IsNullOrEmpty(payment.ArpaPaymentId))
            return payment.ArpaPaymentId;

        // اطمینان از سینک شدن مشتری
        var customerBusinessId = await _syncCustomerUseCase.ExecuteAsync(payment.CustomerId);

        // اطمینان از سینک شدن سفارش (اگر وجود دارد)
        string? arpaOrderId = null;
        if (payment.OrderId > 0)
        {
            arpaOrderId = await _syncOrderUseCase.ExecuteAsync(payment.OrderId);
        }

        var arpaPaymentDto = new ArpaPaymentDto
        {
            CustomerBusinessId = customerBusinessId,
            OrderId = arpaOrderId,
            Amount = payment.Amount,
            PaymentMethod = (int)payment.PaymentMethod,
            TransactionId = payment.TransactionId,
            ReferenceNumber = payment.ReferenceNumber,
            PaymentDate = payment.PaymentDate,
            Description = payment.Description
        };

        var arpaPaymentId = await _arpaApiService.SendPaymentToArpaAsync(arpaPaymentDto);

        payment.ArpaPaymentId = arpaPaymentId;
        payment.IsSyncedWithArpa = true;
        payment.LastSyncDate = DateTime.UtcNow;
        payment.UpdatedAt = DateTime.UtcNow;

        await _paymentRepository.UpdateAsync(payment);

        return arpaPaymentId;
    }
}