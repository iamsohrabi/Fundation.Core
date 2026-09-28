using Fundation.Abstractions.Scheduling;
using MediatR;

namespace Fundation.Core.Scheduler;

public class NullScheduler : IScheduler
{
    public void Enqueue<T>(T request)
        where T : class, IRequest
    {
    }

    public void Enqueue(object request)
    {
    }

    public void Schedule<T>(T request, DateTimeOffset scheduleAt)
        where T : class, IRequest
    {
    }

    public void Schedule(object request, DateTimeOffset scheduleAt)
    {
    }

    public void ScheduleRecurring<T>(T request, string cornExpression)
        where T : class, IRequest
    {
    }

    public void ScheduleRecurring(object request, string cornExpression)
    {
    }
}
