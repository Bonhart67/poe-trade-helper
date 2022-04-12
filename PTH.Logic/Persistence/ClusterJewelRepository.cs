using MongoDB.Bson;
using MongoDB.Driver;
using PTH.Logic.Http;

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
        var previews = _database.GetCollection<BsonDocument>("previews");
        
        // foreach (var variant in variants)
        // {
        //     var details = (await GetClusterDetails(variant)).LastOrDefault();
        //     var filter = Builders<BsonDocument>.Filter.Eq("variant_name", variant);
        //     var update = Builders<BsonDocument>.Update.Set("price_avg", details.AveragePriceOfFirstTen);
        //     await previews.UpdateOneAsync(filter, update);
        // }
    }

    public async Task<IEnumerable<ClusterVariantPreview>> GetClusterPreviews()
    {
        var previews = _database.GetCollection<BsonDocument>("previews");
        return (await previews.FindAsync(Builders<BsonDocument>.Filter.Empty)).ToEnumerable()
            .Select(p => new ClusterVariantPreview
            {
                VariantName = p.GetElement("variant_name").Value.AsString,
                AveragePrice = p.GetElement("price_avg").Value.AsDouble
            }).ToList();
    }

    public async Task CreateClusterDetails()
    {
        var details = _database.GetCollection<BsonDocument>("details");
        foreach (var request in (await _csvReader.ReadLines()).Take(2))
        {
            var result = await _httpQuery.FetchOne(request);
            if (result is not null)
            {
                await details.InsertOneAsync(new BsonDocument
                {
                    {"type_name", result.Type},
                    {"variant_name", result.Name},
                    {"price_min", result.MinPrice},
                    {"price_avg", result.AveragePriceOfFirstTen},
                    {"date", result.Date},
                });
            }

            await Task.Delay(10000);
        }
    }

    public async Task<IEnumerable<ClusterVariantDetail>> GetClusterDetails(string? variantName = null)
    {
        var details = _database.GetCollection<BsonDocument>("details");
        var filter = variantName is null
            ? Builders<BsonDocument>.Filter.Empty
            : Builders<BsonDocument>.Filter.Eq("variant_name", variantName);
        return (await details.FindAsync(filter)).ToEnumerable()
            .Select(p => new ClusterVariantDetail
            {
                Type = p.GetElement("type_name").Value.AsString,
                Name = p.GetElement("variant_name").Value.AsString,
                MinPrice = p.GetElement("price_min").Value.AsDouble,
                AveragePriceOfFirstTen = p.GetElement("price_avg").Value.AsDouble,
                Date = p.GetElement("date").Value.AsBsonDateTime.ToLocalTime()
            }).ToList();

    }
}