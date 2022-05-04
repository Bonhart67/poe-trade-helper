namespace PTH.Domain.Queries;

public class ClusterVariantDetail
{
    public Guid Id;
    public string Type { get; set; }
    public string Name { get; set; }
    public double MinPrice { get; set; }
    public double AveragePriceOfFirstTen { get; set; }
    public DateTime Date { get; set; }
}