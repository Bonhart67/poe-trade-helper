namespace PTH.Logic.Http;

public interface ICsvReader
{
    Task<IReadOnlyList<ClusterVariantRequestData>> ReadLines();
}