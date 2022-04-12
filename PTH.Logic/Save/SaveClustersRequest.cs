using MediatR;
using PTH.Logic.Http;

namespace PTH.Logic;

public record SaveClustersRequest : IRequest;

public class SaveClusterRequestHandler : IRequestHandler<SaveClustersRequest>
{
    public async Task<Unit> Handle(SaveClustersRequest request, CancellationToken cancellationToken)
    {
        
        return Unit.Value;
    }
}