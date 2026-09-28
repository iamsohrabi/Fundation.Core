using Fundation.Abstractions.CQRS.Command;
using Fundation.Abstractions.Scheduling;

namespace Fundation.Core.CQRS.Command;

public class NullCommandScheduler : ICommandScheduler
{
    public Task ScheduleAsync(IInternalCommand internalCommandCommand, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task ScheduleAsync(IInternalCommand[] internalCommandCommands, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
