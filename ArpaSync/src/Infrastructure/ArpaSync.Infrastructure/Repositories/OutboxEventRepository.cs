using ArpaSync.Application.Interfaces;
using ArpaSync.Domain.Entities;
using ArpaSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArpaSync.Infrastructure.Repositories;

public class OutboxEventRepository : IOutboxEventRepository
{
    private readonly AppDbContext _context;

    public OutboxEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OutboxEvent>> GetPendingEventsAsync()
    {
        return await _context.OutboxEvents
            .Where(e => e.Status == OutboxEventStatus.Pending && 
                       (e.NextRetryAt == null || e.NextRetryAt <= DateTime.UtcNow))
            .OrderBy(e => e.CreatedAt)
            .Take(100) // Process max 100 events at a time
            .ToListAsync();
    }

    public async Task<OutboxEvent> AddAsync(OutboxEvent outboxEvent)
    {
        outboxEvent.CreatedAt = DateTime.UtcNow;
        
        _context.OutboxEvents.Add(outboxEvent);
        await _context.SaveChangesAsync();
        return outboxEvent;
    }

    public async Task<OutboxEvent> UpdateAsync(OutboxEvent outboxEvent)
    {
        _context.OutboxEvents.Update(outboxEvent);
        await _context.SaveChangesAsync();
        return outboxEvent;
    }

    public async Task DeleteAsync(Guid id)
    {
        var outboxEvent = await _context.OutboxEvents.FindAsync(id);
        if (outboxEvent != null)
        {
            _context.OutboxEvents.Remove(outboxEvent);
            await _context.SaveChangesAsync();
        }
    }
}