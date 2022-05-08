using MongoDB.Driver;
using Newtonsoft.Json;
using PTH.Domain.Queries;

namespace PTH.Logic.Persistence;

public class ClusterJewelBackupRepository : IClusterJewelBackupRepository
{
    private readonly IMongoDatabase _database;
    private readonly IClusterJewelRepository _clusterJewelRepository;
    private readonly ICurrencyPriceRepository _currencyPriceRepository;
    private readonly string _detailsBackupPath = $"{Environment.CurrentDirectory}/Data/details-backup.json";

    public ClusterJewelBackupRepository(
        IMongoClient client,
        ICurrencyPriceRepository currencyPriceRepository,
        IClusterJewelRepository clusterJewelRepository)
    {
        _currencyPriceRepository = currencyPriceRepository;
        _clusterJewelRepository = clusterJewelRepository;
        _database = client.GetDatabase("clusters");
    }

    public async Task BackupCurrentClusterDetails()
    {
        var details = _database.GetCollection<ClusterVariantDetail>("details");
        var jsonData = JsonConvert.SerializeObject(details.AsQueryable());
        await File.WriteAllTextAsync(_detailsBackupPath, jsonData);
    }

    public async Task FeedAllData()
    {
        await _currencyPriceRepository.UpdatePrices();
        await Task.Run(FeedClusterVariantDetails)
            .ContinueWith(_ => _clusterJewelRepository.UpdateClusterPreviews());
    }

    private async Task FeedClusterVariantDetails()
    {
        var details = _database.GetCollection<ClusterVariantDetail>("details");
        var jsonData = await File.ReadAllTextAsync(_detailsBackupPath);
        await details.InsertManyAsync(JsonConvert.DeserializeObject<List<ClusterVariantDetail>>(jsonData));
    }
}