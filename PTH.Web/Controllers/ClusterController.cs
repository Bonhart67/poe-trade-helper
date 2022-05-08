using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PTH.Domain.Queries;

namespace PTH.Web.Controllers;

public class ClusterController : Controller
{
    private readonly HttpClient _client;

    public ClusterController(IHttpClientFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "C# Program");
    }

    public IActionResult ClusterTypes()
    {
        return View(new List<string>
        {
            "attack", "axe-sword", "bow", "chaos", "cold", "dagger-claw",
            "dual", "elemental", "fire", "lightning", "mace-staff", "minion",
            "physical", "shield", "spell", "two-handed", "wand"
        });
    }
    
    public async Task<IActionResult> ClusterVariants(string type)
    {
        var response = await _client.GetStringAsync($"http://localhost:5001/api/Cluster/GetVariantPreviewsOfType/{type}");
        return View(JsonConvert.DeserializeObject<IEnumerable<ClusterVariantPreview>>(response));
    }
    
    public async Task<IActionResult> ClusterVariantDetails(string variant)
    {
        var response = await _client.GetStringAsync($"http://localhost:5001/api/Cluster/GetVariantDetails/{variant}");
        return View(JsonConvert.DeserializeObject<IEnumerable<ClusterVariantDetail>>(response));
    }
}