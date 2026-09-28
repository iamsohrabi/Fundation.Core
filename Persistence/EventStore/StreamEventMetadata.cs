using Fundation.Abstractions.Persistence.EventStore;

namespace Fundation.Core.Persistence.EventStore;

public record StreamEventMetadata(string EventId, long StreamPosition) : IStreamEventMetadata
{
    public long? LogPosition { get; }
}
