using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using PTH.Domain;

namespace PTH.Logic.Persistence;

public interface ICurrencyPriceRepository
{
    Task UpdatePrices();
    Task<double> GetPriceOfInExalt(string currency);
}

public class CurrencyPriceRepository : ICurrencyPriceRepository
{
    private readonly IMongoDatabase _database;
    private readonly HttpClient _client;

    public CurrencyPriceRepository(IMongoClient client)
    {
        _database = client.GetDatabase("currencies");
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "C# Program");
    }
    public async Task UpdatePrices()
    {
        var prices = _database.GetCollection<CurrencyInChaos>("prices");
        var currencies = await GetCurrenciesWithChaosEquivalent();
        if (await prices.EstimatedDocumentCountAsync() == 0)
        {
            await prices.InsertManyAsync(currencies);
            return;
        }
        foreach (var currency in currencies)
        {
            var filter = Builders<CurrencyInChaos>.Filter.Eq(c => c.ShortName, currency.ShortName);
            var update = Builders<CurrencyInChaos>.Update
                .Set(c => c.ShortName, currency.ShortName)
                .Set(c => c.Amount, currency.Amount);
            await prices.UpdateOneAsync(filter, update);
        }
    }

    public async Task<double> GetPriceOfInExalt(string currency)
    {
        var prices = _database.GetCollection<CurrencyInChaos>("prices");
        var priceOfExaltInChaos = await prices.Find(Builders<CurrencyInChaos>.Filter.Eq(c => c.ShortName, "exalted")).FirstOrDefaultAsync();
        var priceOfChaosInExalt = 1 / priceOfExaltInChaos.Amount;
        if (currency is "chaos")
        {
            return priceOfChaosInExalt;
        }
        var priceOfCurrentInChaos = await prices.Find(Builders<CurrencyInChaos>.Filter.Eq(c => c.ShortName, currency)).FirstOrDefaultAsync();
        return priceOfChaosInExalt * priceOfCurrentInChaos.Amount;
    }

    private async Task<IEnumerable<CurrencyInChaos>> GetCurrenciesWithChaosEquivalent()
    {
        var league = "Archnemesis";
        var poeNinjaRequestUri = $"https://poe.ninja/api/data/CurrencyOverview?league={league}&type=Currency&language=en";
        var response = await _client.GetStringAsync(poeNinjaRequestUri);
        var details = JObject.Parse(response)["currencyDetails"]
            .Where(t => t["tradeId"]?.Value<string?>() is not null)
            .ToDictionary(
                k => k["name"].Value<string>(),
                v => v["tradeId"].Value<string?>());
        return JObject.Parse(response)["lines"]
            .Where(t => details.ContainsKey(t["currencyTypeName"].Value<string>()))
            .Select(t => new CurrencyInChaos
            {
                ShortName = details[t["currencyTypeName"].Value<string>()],
                Amount = t["chaosEquivalent"].Value<double>()
            });
    }
}