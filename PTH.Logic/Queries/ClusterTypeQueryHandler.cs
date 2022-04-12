using MediatR;

namespace PTH.Logic;

public record GetVariantsRequest : IRequest<IEnumerable<ClusterVariantPreview>>;

public class ClusterTypeQueryHandler : IRequestHandler<GetVariantsRequest, IEnumerable<ClusterVariantPreview>>
{
    public async Task<IEnumerable<ClusterVariantPreview>> Handle(GetVariantsRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}