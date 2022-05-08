using MediatR;
using PTH.Logic.Update;
using Quartz;

namespace PTH.API.Scheduling;

public class UpdateClusterPreviewsJob : IJob
{
    private readonly IMediator _mediator;

    public UpdateClusterPreviewsJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new UpdateClusterPreviewsRequest());
    }
}