using Fundation.Abstractions.Messaging;

namespace Fundation.Core.Messaging;

public record Message : IMessage
{
    public Guid MessageId => Guid.NewGuid();
    public DateTime Created { get; } = DateTime.Now;
}
