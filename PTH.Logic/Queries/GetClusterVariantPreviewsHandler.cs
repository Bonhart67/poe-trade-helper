using MediatR;
using PTH.Domain.Queries;
using PTH.Logic.Persistence;

namespace PTH.Logic.Queries;

public record GetClusterVariantPreviewsRequest(string Type) : IRequest<IEnumerable<ClusterVariantPreview>>;

public class GetClusterVariantPreviewsHandler : IRequestHandler<GetClusterVariantPreviewsRequest, IEnumerable<ClusterVariantPreview>>
{
    private readonly IClusterJewelRepository _repository;

    public GetClusterVariantPreviewsHandler(IClusterJewelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ClusterVariantPreview>> Handle(GetClusterVariantPreviewsRequest request, CancellationToken cancellationToken)
    {
        return await _repository.GetClusterPreviews(request.Type);
    }
}