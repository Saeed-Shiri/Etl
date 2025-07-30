using ArpaSync.Domain.Entities;

namespace ArpaSync.Application.Interfaces;

public interface IOutboxEventRepository
{
    Task<List<OutboxEvent>> GetPendingEventsAsync();
    Task<OutboxEvent> AddAsync(OutboxEvent outboxEvent);
    Task<OutboxEvent> UpdateAsync(OutboxEvent outboxEvent);
    Task DeleteAsync(Guid id);
}