using PTH.Domain.Queries;

namespace PTH.Logic.Other;

public interface IHttpQuery
{
    Task<ClusterVariantDetail?> FetchOne(ClusterVariantRequestData requestData);
}