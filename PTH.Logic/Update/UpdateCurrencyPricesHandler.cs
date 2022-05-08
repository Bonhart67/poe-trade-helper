using MediatR;
using PTH.Logic.Persistence;

namespace PTH.Logic.Update;

public record UpdateCurrencyPricesRequest : IRequest;

public class UpdateCurrencyPricesHandler : IRequestHandler<UpdateCurrencyPricesRequest>
{
    private readonly ICurrencyPriceRepository _repository;

    public UpdateCurrencyPricesHandler(ICurrencyPriceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateCurrencyPricesRequest request, CancellationToken cancellationToken)
    {
        await _repository.UpdatePrices();
        return Unit.Value;
    }
}