using PTH.Domain.Queries;

namespace PTH.Logic.Other;

public interface ICsvReader
{
    Task<IReadOnlyList<ClusterVariantRequestData>> ReadLines();
}