using MediatR;
using PTH.Domain.Queries;
using PTH.Logic.Persistence;

namespace PTH.Logic.Queries;

public record GetClusterVariantDetailsRequest(string Variant) : IRequest<IEnumerable<ClusterVariantDetail>>;

public class GetClusterVariantDetailsHandler : IRequestHandler<GetClusterVariantDetailsRequest, IEnumerable<ClusterVariantDetail>>
{
    private readonly IClusterJewelRepository _repository;

    public GetClusterVariantDetailsHandler(IClusterJewelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ClusterVariantDetail>> Handle(GetClusterVariantDetailsRequest request, CancellationToken cancellationToken)
    {
        return await _repository.GetClusterDetails(request.Variant);
    }
}