using MongoDB.Driver;
using PTH.Domain.Queries;
using PTH.Logic.Other;

namespace PTH.Logic.Persistence;

public class ClusterJewelRepository : IClusterJewelRepository
{
    private readonly IMongoDatabase _database;
    private readonly IHttpQuery _httpQuery;
    private readonly ICsvReader _csvReader;

    public ClusterJewelRepository(
        IMongoClient client, 
        IHttpQuery httpQuery, 
        ICsvReader csvReader)
    {
        _httpQuery = httpQuery;
        _csvReader = csvReader;
        _database = client.GetDatabase("clusters");
    }

    public async Task UpdateClusterPreviews()
    {
        var previews = _database.GetCollection<ClusterVariantPreview>("previews");
        var details = _database.GetCollection<ClusterVariantDetail>("details");
        var result = details.AsQueryable().ToList()
            .GroupBy(d => d.Name)
            .Select(g => g.MaxBy(d => d.Date))
            .Select(d => new ClusterVariantPreview()
            {
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
        foreach (var request in (await _csvReader.ReadLines()).Take(20))
        {
            var result = await _httpQuery.FetchOne(request);
            if (result is not null)
            {
                await details.InsertOneAsync(result);
            }

            await Task.Delay(10000);
        }
    }

    public async Task<IEnumerable<ClusterVariantDetail>> GetClusterDetails(string? variantName = null)
    {
        var details = _database.GetCollection<ClusterVariantDetail>("details");
        var filter = variantName is null
            ? Builders<ClusterVariantDetail>.Filter.Empty
            : Builders<ClusterVariantDetail>.Filter.Eq("Name", variantName);
        return (await details.FindAsync(filter)).ToEnumerable();
    }
}