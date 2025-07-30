namespace ArpaSync.Domain.Entities;

public class OutboxEvent
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public OutboxEventStatus Status { get; set; }
    public int RetryCount { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime? NextRetryAt { get; set; }
}

public enum OutboxEventStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4
}