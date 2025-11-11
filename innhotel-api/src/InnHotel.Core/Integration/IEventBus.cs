namespace InnHotel.Core.Integration;

/// <summary>
/// Interface for the event bus that handles integration events
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an integration event asynchronously
    /// </summary>
    /// <param name="event">The integration event to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes multiple integration events asynchronously
    /// </summary>
    /// <param name="events">The integration events to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task PublishAsync(IEnumerable<IIntegrationEvent> events, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes to an integration event
    /// </summary>
    /// <typeparam name="TEvent">The event type to subscribe to</typeparam>
    /// <param name="handler">The event handler</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SubscribeAsync<TEvent>(Func<TEvent, Task> handler, CancellationToken cancellationToken = default) 
        where TEvent : IIntegrationEvent;

    /// <summary>
    /// Unsubscribes from an integration event
    /// </summary>
    /// <typeparam name="TEvent">The event type to unsubscribe from</typeparam>
    /// <param name="handler">The event handler</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UnsubscribeAsync<TEvent>(Func<TEvent, Task> handler, CancellationToken cancellationToken = default) 
        where TEvent : IIntegrationEvent;
}