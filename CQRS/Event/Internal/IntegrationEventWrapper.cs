using Fundation.Abstractions.CQRS.Event.Internal;
using Fundation.Core.Messaging;

namespace Fundation.Core.CQRS.Event.Internal;

public record IntegrationEventWrapper<TDomainEventType>
    : IntegrationEvent
    where TDomainEventType : IDomainEvent;
