using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using PTH.Logic.Http;

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
        var previews = _database.GetCollection<BsonDocument>("previews");
        if (await previews.EstimatedDocumentCountAsync() > 0)
        {
            return;
        }
        var variants = await _csvReader.ReadLines();
        foreach (var variant in variants)
        {
            await previews.InsertOneAsync(new BsonDocument()
            {
                {"type_name", variant.Type},
                {"variant_name", variant.Variant},
                {"price_avg", (double)0}
            });
        }
    }

    public Task FeedClusterVariantDetails()
    {
        throw new NotImplementedException();
    }
}