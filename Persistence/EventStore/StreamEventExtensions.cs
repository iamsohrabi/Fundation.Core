using Fundation.Abstractions.CQRS.Event.Internal;
using Fundation.Abstractions.Persistence.EventStore;
using Fundation.Core.Utils;

namespace Fundation.Core.Persistence.EventStore;

public static class StreamEventExtensions
{
    public static IStreamEvent ToStreamEvent(
        this IDomainEvent domainEvent,
        IStreamEventMetadata? metadata)
    {
        return ReflectionUtilities.CreateGenericType(
            typeof(StreamEvent<>),
            new[] { domainEvent.GetType() },
            domainEvent,
            metadata);
    }
}
