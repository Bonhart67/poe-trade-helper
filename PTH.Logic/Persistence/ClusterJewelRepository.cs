using MongoDB.Driver;
using PTH.Domain.Queries;
using PTH.Logic.Other;

namespace PTH.Logic.Persistence;

public class ClusterJewelRepository : IClusterJewelRepository
{
    private readonly IMongoDatabase _database;
    private readonly IHttpQuery _httpQuery;
    private readonly IJsonReader _jsonReader;

    public ClusterJewelRepository(
        IMongoClient client, 
        IHttpQuery httpQuery,
        IJsonReader jsonReader)
    {
        _httpQuery = httpQuery;
        _jsonReader = jsonReader;
        _database = client.GetDatabase("clusters");
    }

    public async Task UpdateClusterPreviews()
    {
        var previews = _database.GetCollection<ClusterVariantPreview>("previews");
        if (await previews.EstimatedDocumentCountAsync() == 0)
        {
            var clusterVariantPreviews = (await _jsonReader.Read<ClusterVariantRequestData>())
                .Select(r => new ClusterVariantPreview
            {
                Type = r.Type,
                VariantName = r.Variant,
                AveragePrice = 0d
            });
            await previews.InsertManyAsync(clusterVariantPreviews);
        }
        var details = _database.GetCollection<ClusterVariantDetail>("details");
        var result = details.AsQueryable().ToList()
            .GroupBy(d => d.Name)
            .Select(g => g.MaxBy(d => d.Date))
            .Select(d => new ClusterVariantPreview
            {
                Type = d.Type,
                VariantName = d.Name,
                AveragePrice = d.AveragePriceOfFirstTen
            });
        
        foreach (var variant in result)
        {
            var filter = Builders<ClusterVariantPreview>.Filter.Eq("VariantName", variant.VariantName);
            var update = Builders<ClusterVariantPreview>.Update.Set("AveragePrice", variant.AveragePrice);
            await previews.UpdateOneAsync(filter, update);
        }
    }

    public async Task<IEnumerable<ClusterVariantPreview>> GetClusterPreviews(string type)
    {
        var previews = _database.GetCollection<ClusterVariantPreview>("previews");
        return (await previews.FindAsync(Builders<ClusterVariantPreview>.Filter.Eq("Type", type))).ToEnumerable();
    }

    public async Task CreateClusterDetails()
    {
        var details = _database.GetCollection<ClusterVariantDetail>("details");
        foreach (var request in (await _jsonReader.Read<ClusterVariantRequestData>())!)
        {
            var result = await _httpQuery.FetchOne(request);
            if (result is not null)
            {
                await details.InsertOneAsync(result);
            }
        }
    }

    public async Task<IEnumerable<ClusterVariantDetail>> GetClusterDetails(string? variantName = null)
    {
        var details = _database.GetCollection<ClusterVariantDetail>("details");
        var result = await details.FindAsync(d => variantName == null || d.Name == variantName);
        return result.ToEnumerable();
    }
}