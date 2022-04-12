namespace PTH.Logic;

public record ClusterVariantDetail
{
    public string Type { get; set; }
    public string Name { get; set; }
    public double MinPrice { get; set; }
    public double AveragePriceOfFirstTen { get; set; }
    public DateTime Date { get; set; }
}