using Fundation.Abstractions.CQRS.Command;
using Fundation.Core.Types;

namespace Fundation.Core.CQRS.Command;

public abstract record InternalCommand : IInternalCommand
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTime OccurredOn { get; protected set; } = DateTime.Now;

    public string Type { get { return TypeMapper.GetFullTypeName(GetType()); } }
}
