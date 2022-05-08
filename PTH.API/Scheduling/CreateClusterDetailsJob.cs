using MediatR;
using PTH.Logic.Create;
using Quartz;

namespace PTH.API.Scheduling;

public class CreateClusterDetailsJob : IJob
{
    private readonly IMediator _mediator;

    public CreateClusterDetailsJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new CreateClusterDetailsRequest());
    }
}