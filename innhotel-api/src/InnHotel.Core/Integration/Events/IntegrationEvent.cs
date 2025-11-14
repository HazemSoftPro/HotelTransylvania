namespace InnHotel.Core.Integration.Events;

/// <summary>
/// Base implementation for integration events
/// </summary>
public abstract class IntegrationEvent : IIntegrationEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public string EventType { get; }
    public string AggregateId { get; }
    public Dictionary<string, object> Metadata { get; }

    protected IntegrationEvent(string aggregateId, Dictionary<string, object>? metadata = null)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        EventType = GetType().Name;
        AggregateId = aggregateId;
        Metadata = metadata ?? new Dictionary<string, object>();
    }
}