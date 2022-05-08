using MediatR;
using PTH.Logic.Persistence;

namespace PTH.Logic.Create;

public record CreateClusterDetailsRequest : IRequest;

public class CreateClusterDetailsHandler : IRequestHandler<CreateClusterDetailsRequest>
{
    private readonly IClusterJewelRepository _repository;

    public CreateClusterDetailsHandler(IClusterJewelRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CreateClusterDetailsRequest request, CancellationToken cancellationToken)
    {
        await _repository.CreateClusterDetails();
        return Unit.Value;
    }
}