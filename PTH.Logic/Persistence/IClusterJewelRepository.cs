namespace PTH.Logic.Persistence;

public interface IClusterJewelRepository
{
    Task UpdateClusterPreviews();
    Task<IEnumerable<ClusterVariantPreview>> GetClusterPreviews();
    Task CreateClusterDetails();
    Task<IEnumerable<ClusterVariantDetail>> GetClusterDetails(string? variantName = null);
}