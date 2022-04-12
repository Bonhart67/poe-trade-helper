using MediatR;

namespace PTH.Logic;

public record GetVariantDetailsRequest(string Variant) : IRequest<IEnumerable<ClusterVariantDetail>>;

public class ClusterVariantQueryHandler : IRequestHandler<GetVariantDetailsRequest, IEnumerable<ClusterVariantDetail>>
{
    public Task<IEnumerable<ClusterVariantDetail>> Handle(GetVariantDetailsRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}