using PTH.Domain.Queries;

namespace PTH.Logic.Persistence;

public interface IClusterJewelRepository
{
    Task UpdateClusterPreviews();
    Task<IEnumerable<ClusterVariantPreview>> GetClusterPreviews(string type);
    Task CreateClusterDetails();
    Task<IEnumerable<ClusterVariantDetail>> GetClusterDetails(string? variantName = null);
}