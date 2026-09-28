using Fundation.Abstractions.CQRS.Event.Internal;

namespace Fundation.Core.CQRS.Event.Internal;

public abstract record DomainNotificationEvent : Event, IDomainNotificationEvent;
