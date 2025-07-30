using ArpaSync.Application.Interfaces;
using ArpaSync.Application.UseCases;
using ArpaSync.Domain.Entities;
using ArpaSync.Domain.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ArpaSync.Application.Services;

public class OutboxEventProcessorService : BackgroundService
{
    private readonly IOutboxEventRepository _outboxEventRepository;
    private readonly SyncCustomerWithArpaUseCase _syncCustomerUseCase;
    private readonly SyncOrderWithArpaUseCase _syncOrderUseCase;
    private readonly SyncPaymentWithArpaUseCase _syncPaymentUseCase;
    private readonly ILogger<OutboxEventProcessorService> _logger;

    public OutboxEventProcessorService(
        IOutboxEventRepository outboxEventRepository,
        SyncCustomerWithArpaUseCase syncCustomerUseCase,
        SyncOrderWithArpaUseCase syncOrderUseCase,
        SyncPaymentWithArpaUseCase syncPaymentUseCase,
        ILogger<OutboxEventProcessorService> logger)
    {
        _outboxEventRepository = outboxEventRepository;
        _syncCustomerUseCase = syncCustomerUseCase;
        _syncOrderUseCase = syncOrderUseCase;
        _syncPaymentUseCase = syncPaymentUseCase;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingEventsAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing outbox events");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }

    private async Task ProcessPendingEventsAsync()
    {
        var pendingEvents = await _outboxEventRepository.GetPendingEventsAsync();
        
        foreach (var outboxEvent in pendingEvents)
        {
            try
            {
                outboxEvent.Status = OutboxEventStatus.Processing;
                await _outboxEventRepository.UpdateAsync(outboxEvent);

                await ProcessEventAsync(outboxEvent);

                outboxEvent.Status = OutboxEventStatus.Completed;
                outboxEvent.ProcessedAt = DateTime.UtcNow;
                await _outboxEventRepository.UpdateAsync(outboxEvent);

                _logger.LogInformation("Successfully processed outbox event {EventId} of type {EventType}", 
                    outboxEvent.Id, outboxEvent.EventType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox event {EventId} of type {EventType}", 
                    outboxEvent.Id, outboxEvent.EventType);

                outboxEvent.Status = OutboxEventStatus.Failed;
                outboxEvent.RetryCount++;
                outboxEvent.ErrorMessage = ex.Message;
                outboxEvent.NextRetryAt = DateTime.UtcNow.AddMinutes(Math.Pow(2, outboxEvent.RetryCount));
                
                await _outboxEventRepository.UpdateAsync(outboxEvent);
            }
        }
    }

    private async Task ProcessEventAsync(OutboxEvent outboxEvent)
    {
        switch (outboxEvent.EventType)
        {
            case nameof(CustomerCreatedEvent):
                var customerEvent = JsonSerializer.Deserialize<CustomerCreatedEvent>(outboxEvent.EventData);
                if (customerEvent != null)
                {
                    await _syncCustomerUseCase.ExecuteAsync(customerEvent.CustomerId);
                }
                break;

            case nameof(OrderPaidEvent):
                var orderEvent = JsonSerializer.Deserialize<OrderPaidEvent>(outboxEvent.EventData);
                if (orderEvent != null)
                {
                    await _syncOrderUseCase.ExecuteAsync(orderEvent.OrderId);
                }
                break;

            case nameof(PaymentCompletedEvent):
                var paymentEvent = JsonSerializer.Deserialize<PaymentCompletedEvent>(outboxEvent.EventData);
                if (paymentEvent != null)
                {
                    await _syncPaymentUseCase.ExecuteAsync(paymentEvent.PaymentId);
                }
                break;

            default:
                throw new InvalidOperationException($"Unknown event type: {outboxEvent.EventType}");
        }
    }
}