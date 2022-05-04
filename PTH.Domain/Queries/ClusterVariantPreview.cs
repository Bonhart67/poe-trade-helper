namespace PTH.Domain.Queries;

public class ClusterVariantPreview
{
    public Guid Id;
    public string Type { get; set; }
    public string VariantName { get; set; }
    public double AveragePrice { get; set; }
}