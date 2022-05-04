using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTH.Domain.Queries;
using PTH.Logic.Create;
using PTH.Logic.Queries;

namespace PTH.API.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class ClusterController  : ControllerBase
{
    private readonly IMediator _mediator;

    public ClusterController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet(Name="GetVariantDetails")]
    [Route("/api/[controller]/details/{variant}")]
    public async Task<IEnumerable<ClusterVariantDetail>> GetVariantDetails([FromBody] string variant)
    {
        return await _mediator.Send(new GetClusterVariantDetailsRequest(variant));
    }

    [HttpGet(Name="GetVariantPreviews")]
    [Route("/api/[controller]/previews/{type}")]
    public async Task<IEnumerable<ClusterVariantPreview>> GetVariantPreviewsOfType([FromBody] string type)
    {
        return await _mediator.Send(new GetClusterVariantPreviewsRequest(type));
    }

    [HttpPost(Name="SaveAllClusterVariants")]
    [Route("/api/[controller]/save-details")]
    public async Task<IActionResult> SaveAllClusterVariants()
    {
        await _mediator.Send(new CreateClusterDetailsRequest());
        return Ok();
    }

    [HttpPost(Name="UpdateAllClusterPreviews")]
    [Route("/api/[controller]/update-previews")]
    public async Task<IActionResult> UpdateAllClusterPreviews()
    {
        await _mediator.Send(new UpdateClusterPreviewsRequest());
        return Ok();
    }
}
