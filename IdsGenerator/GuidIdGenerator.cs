using Fundation.Abstractions.Core;

namespace Fundation.Core.IdsGenerator;

public class GuidIdGenerator : IIdGenerator<Guid>
{
    public Guid New()
    {
       return Guid.NewGuid();
    }
}
