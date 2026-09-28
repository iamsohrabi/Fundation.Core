using Fundation.Abstractions.CQRS.Event.Internal;

namespace Fundation.Core.CQRS.Event.Internal;

public record DomainNotificationEventWrapper<TDomainEventType>(TDomainEventType DomainEvent) : DomainNotificationEvent
    where TDomainEventType : IDomainEvent;
