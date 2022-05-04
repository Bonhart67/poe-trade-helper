using MongoDB.Driver;
using PTH.Domain.Queries;
using PTH.Logic.Other;

namespace PTH.Logic.Persistence;

public interface IClusterJewelBackupRepository
{
    Task BackupCurrentClusterDetails();
    Task FeedClusterVariantPreviewsIfEmpty();
    Task FeedClusterVariantDetails();
}

public class ClusterJewelBackupRepository : IClusterJewelBackupRepository
{
    private readonly IMongoDatabase _database;
    private readonly ICsvReader _csvReader;

    public ClusterJewelBackupRepository(IMongoClient client, ICsvReader csvReader)
    {
        _csvReader = csvReader;
        _database = client.GetDatabase("clusters");
    }

    public Task BackupCurrentClusterDetails()
    {
        throw new NotImplementedException();
    }

    public async Task FeedClusterVariantPreviewsIfEmpty()
    {
        var previews = _database.GetCollection<ClusterVariantPreview>("previews");
        if (await previews.EstimatedDocumentCountAsync() > 0)
        {
            return;
        }
        await previews.InsertManyAsync((await _csvReader.ReadLines()).Select(v => new ClusterVariantPreview
        {
            VariantName = v.Variant,
            AveragePrice = 0d
        }));
    }

    public Task FeedClusterVariantDetails()
    {
        throw new NotImplementedException();
    }
}