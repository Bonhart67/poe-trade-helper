namespace PTH.Logic.Persistence;

public interface ICurrencyPriceRepository
{
    Task UpdatePrices();
    Task<double> GetPriceOfInExalt(string currency);
}