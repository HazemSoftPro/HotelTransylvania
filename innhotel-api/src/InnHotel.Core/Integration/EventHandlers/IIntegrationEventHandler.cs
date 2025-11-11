namespace InnHotel.Core.Integration.EventHandlers;

/// <summary>
/// Base interface for integration event handlers
/// </summary>
/// <typeparam name="TEvent">The event type this handler handles</typeparam>
public interface IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent
{
    /// <summary>
    /// Handles the integration event
    /// </summary>
    /// <param name="event">The integration event to handle</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}