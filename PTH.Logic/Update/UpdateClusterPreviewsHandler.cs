using MediatR;
using PTH.Logic.Persistence;

namespace PTH.Logic.Update;

public record UpdateClusterPreviewsRequest : IRequest;

public class UpdateClusterPreviewsHandler : IRequestHandler<UpdateClusterPreviewsRequest>
{
    private readonly IClusterJewelRepository _repository;

    public UpdateClusterPreviewsHandler(IClusterJewelRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateClusterPreviewsRequest request, CancellationToken cancellationToken)
    {
        await _repository.UpdateClusterPreviews();
        return Unit.Value;
    }
}