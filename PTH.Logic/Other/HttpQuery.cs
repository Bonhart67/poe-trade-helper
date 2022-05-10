using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PTH.Domain.Queries;

namespace PTH.Logic.Other;

public class HttpQuery : IHttpQuery
{
    private const string PoeTradeUri = "https://www.pathofexile.com/api/trade";
    private readonly HttpClient _client;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly IConfiguration _configuration;

    public HttpQuery(ICurrencyConverter currencyConverter, IConfiguration configuration)
    {
        _currencyConverter = currencyConverter;
        _configuration = configuration;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "C# Program");
    }

    ~HttpQuery()
    {
        _client.Dispose();
    }

    public async Task<ClusterVariantDetail?> FetchOne(ClusterVariantRequestData requestData)
    {
        var itemIds = await FetchItemIdsFromTradeSite(requestData.Request);
        if (!itemIds.Any()) return null;
        var requestUri = $"{PoeTradeUri}/fetch/{string.Join(',', itemIds)}";
        var prices = await FetchPrices(requestUri);
        if (prices.Any() == false) return null;
        return new ClusterVariantDetail
        {
            Type = requestData.Type,
            Name = requestData.Variant,
            MinPrice = Math.Round(prices.Min(), 2),
            AveragePriceOfFirstTen = Math.Round(prices.Average(), 2),
            Date = DateTime.Now
        };
    }

    private async Task<IEnumerable<string?>> FetchItemIdsFromTradeSite(string requestBody)
    {
        var tradeSearchRequestUri = $"{PoeTradeUri}/search/{_configuration["CurrentLeague"]}";
        using var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), tradeSearchRequestUri);
        requestMessage.Content = new StringContent(requestBody);
        requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        var responseBody = await (await _client.SendAsync(requestMessage)).Content.ReadAsStringAsync();
        var response = JObject.Parse(responseBody)["result"];
        if (response is null || !response.HasValues)
            return Enumerable.Empty<string>();
        return response.Select(t => t.Value<string>()).Take(10);
    }

    private async Task<IEnumerable<double>> FetchPrices(string requestUri)
    {
        try
        {
            var responseBody = await _client.GetStringAsync(requestUri);
            var itemResponse = JObject.Parse(responseBody)["result"]?
                .Select(t => t["listing"]?["price"])
                .Where(t => t is not null);
            return itemResponse?.Select(t => _currencyConverter.ConvertToExalt(
                       t["currency"].Value<string>(),
                       double.Parse(t["amount"].Value<string>())).Result)
                   ?? Enumerable.Empty<double>();
        }
        catch
        {
            return Enumerable.Empty<double>();
        }
    }
}