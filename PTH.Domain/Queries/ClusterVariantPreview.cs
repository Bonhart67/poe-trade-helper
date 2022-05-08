using Newtonsoft.Json;

namespace PTH.Domain.Queries;

public class ClusterVariantPreview
{
    [JsonIgnore]
    public Guid Id;
    public string Type { get; set; }
    public string VariantName { get; set; }
    public double AveragePrice { get; set; }
}