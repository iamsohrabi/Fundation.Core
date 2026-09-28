using Fundation.Abstractions.CQRS;
using Fundation.Abstractions.CQRS.Query;

namespace Fundation.Core.CQRS.Query;

public record ListQuery<TResponse> : IListQuery<TResponse>
    where TResponse : notnull
{
    public IList<string>? Includes { get; init; }
    public IList<FilterModel>? Filters { get; init; }
    public IList<string>? Sorts { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
