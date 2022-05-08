using MediatR;
using PTH.Logic.Update;
using Quartz;

namespace PTH.API.Scheduling;

public class UpdateCurrencyPricesJob : IJob
{
    private readonly IMediator _mediator;

    public UpdateCurrencyPricesJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new UpdateCurrencyPricesRequest());
    }
}