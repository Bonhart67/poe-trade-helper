using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace PTH.Logic.Http;

public class HttpQuery : IHttpQuery
{
    private const string PoeTradeUri = "https://www.pathofexile.com/api/trade";
    private readonly HttpClient _client;

    public HttpQuery()
    {
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
            MinPrice = Math.Round(prices.Min()),
            AveragePriceOfFirstTen = Math.Round(prices.Average(), 2),
            Date = DateTime.Now
        };
    }

    private async Task<IEnumerable<string>> FetchItemIdsFromTradeSite(string requestBody)
    {
        var tradeSearchRequestUri = $"{PoeTradeUri}/search/Archnemesis";
        using var request = new HttpRequestMessage(new HttpMethod("POST"), tradeSearchRequestUri);
        request.Content = new StringContent(requestBody);
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        var responseBody = await (await _client.SendAsync(request)).Content.ReadAsStringAsync();
        var response = JObject.Parse(responseBody)["result"];
        return response.HasValues 
            ? response.Select(t => t.Value<string>()).Take(10) 
            : Enumerable.Empty<string>();
    }

    private async Task<IEnumerable<double>> FetchPrices(string requestUri)
    {
        try
        {
            var itemsResponseBody = await _client.GetStringAsync(requestUri);
            var itemResponse = JObject.Parse(itemsResponseBody)["result"].Select(t => t["listing"]["price"]);
            return itemResponse.Select(t => GetPriceInExalt(
                double.Parse(t["amount"].Value<string>()),
                t["currency"].Value<string>()));
        }
        catch
        {
            return Enumerable.Empty<double>();
        }
    }

    private double GetPriceInExalt(double amount, string currency)
    {
        if (currency.Equals("chaos"))
        {
            amount /= 165;
        }

        return amount;
    }
}