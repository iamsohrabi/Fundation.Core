using Fundation.Abstractions.CQRS.Event;
using Fundation.Abstractions.CQRS.Event.Internal;

namespace Fundation.Core.CQRS.Event;

public class NullDomainEventsAccessor : IDomainEventsAccessor
{
    public IReadOnlyList<IDomainEvent> UnCommittedDomainEvents { get; }
}
