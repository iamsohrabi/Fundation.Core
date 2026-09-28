using Fundation.Abstractions.Messaging;

namespace Fundation.Core.Messaging;

public record IntegrationEvent : Message, IIntegrationEvent;
