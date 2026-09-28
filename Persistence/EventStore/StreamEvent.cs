using Fundation.Abstractions.CQRS.Event.Internal;
using Fundation.Abstractions.Persistence.EventStore;
using Fundation.Core.CQRS.Event;

namespace Fundation.Core.Persistence.EventStore;

public record StreamEvent
    (IDomainEvent Data,  IStreamEventMetadata? Metadata = null) : Event, IStreamEvent;

public record StreamEvent<T>(T Data,  IStreamEventMetadata? Metadata = null)
    : StreamEvent(Data, Metadata), IStreamEvent<T>
    where T : IDomainEvent
{
    public new T Data => (T)base.Data;
}
