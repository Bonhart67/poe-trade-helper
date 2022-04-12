namespace PTH.Logic.Http;

public interface IHttpQuery
{
    Task<ClusterVariantDetail?> FetchOne(ClusterVariantRequestData requestData);
}