namespace InnHotel.Core.Integration.Events;

/// <summary>
/// Base interface for all integration events
/// </summary>
public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
    string AggregateId { get; }
    Dictionary<string, object> Metadata { get; }
}