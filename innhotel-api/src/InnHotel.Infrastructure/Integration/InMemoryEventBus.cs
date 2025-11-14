using InnHotel.Core.Integration;
using InnHotel.Core.Integration.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace InnHotel.Infrastructure.Integration;

/// <summary>
/// In-memory implementation of event bus for development and testing
/// In production, this should be replaced with a message broker like RabbitMQ or Azure Service Bus
/// </summary>
public class InMemoryEventBus : IEventBus
{
    private readonly ILogger<InMemoryEventBus> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, List<Func<IIntegrationEvent, Task>>> _handlers;

    public InMemoryEventBus(
        ILogger<InMemoryEventBus> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _handlers = new ConcurrentDictionary<Type, List<Func<IIntegrationEvent, Task>>>();
    }

    public async Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publishing event {EventType} with ID {EventId}", @event.EventType, @event.EventId);

        var eventType = @event.GetType();
        
        if (_handlers.TryGetValue(eventType, out var handlers))
        {
            var tasks = handlers.Select(async handler =>
            {
                try
                {
                    await handler(@event);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling event {EventType} with ID {EventId}", @event.EventType, @event.EventId);
                    // Continue processing other handlers even if one fails
                }
            });

            await Task.WhenAll(tasks);
        }
        else
        {
            _logger.LogWarning("No handlers registered for event type {EventType}", eventType.Name);
        }

        // Also try to find and execute typed handlers through DI
        await PublishToTypedHandlers(@event, cancellationToken);
    }

    public async Task PublishAsync(IEnumerable<IIntegrationEvent> events, CancellationToken cancellationToken = default)
    {
        var tasks = events.Select(@event => PublishAsync(@event, cancellationToken));
        await Task.WhenAll(tasks);
    }

    public Task SubscribeAsync<TEvent>(Func<TEvent, Task> handler, CancellationToken cancellationToken = default) 
        where TEvent : IIntegrationEvent
    {
        var eventType = typeof(TEvent);
        var wrappedHandler = new Func<IIntegrationEvent, Task>(async (evt) =>
        {
            if (evt is TEvent typedEvent)
            {
                await handler(typedEvent);
            }
        });

        _handlers.AddOrUpdate(eventType, 
            new List<Func<IIntegrationEvent, Task>> { wrappedHandler },
            (key, existing) =>
            {
                existing.Add(wrappedHandler);
                return existing;
            });

        _logger.LogInformation("Subscribed handler for event type {EventType}", eventType.Name);
        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync<TEvent>(Func<TEvent, Task> handler, CancellationToken cancellationToken = default) 
        where TEvent : IIntegrationEvent
    {
        var eventType = typeof(TEvent);
        
        if (_handlers.TryGetValue(eventType, out var handlers))
        {
            var toRemove = handlers.FirstOrDefault(h => h.Target?.Equals(handler) ?? false);
            if (toRemove != null)
            {
                handlers.Remove(toRemove);
                _logger.LogInformation("Unsubscribed handler for event type {EventType}", eventType.Name);
            }
        }

        return Task.CompletedTask;
    }

    private async Task PublishToTypedHandlers(IIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var eventType = @event.GetType();
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

        using var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            try
            {
                var method = handlerType.GetMethod(nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync));
                if (method != null)
                {
                    await (Task)method.Invoke(handler, new object[] { @event, cancellationToken })!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing typed handler for event {EventType}", eventType.Name);
            }
        }
    }
}