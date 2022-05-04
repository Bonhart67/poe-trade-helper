using PTH.Logic.Persistence;

namespace PTH.Logic.Other;

public interface ICurrencyConverter
{
    Task<double> ConvertToExalt(string currency, double amount);
}

public class CurrencyConverter : ICurrencyConverter
{
    private readonly ICurrencyPriceRepository _repository;

    public CurrencyConverter(ICurrencyPriceRepository repository)
    {
        _repository = repository;
    }

    public async Task<double> ConvertToExalt(string currency, double amount)
    {
        return amount * await _repository.GetPriceOfInExalt(currency);
    }
}