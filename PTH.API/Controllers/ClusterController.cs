using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PTH.Domain.Queries;
using PTH.Logic.Create;
using PTH.Logic.Query;
using PTH.Logic.Update;

namespace PTH.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ClusterController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClusterController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("/api/[controller]/[action]/{variant}")]
    public async Task<IEnumerable<ClusterVariantDetail>> GetVariantDetails([FromRoute] string variant)
    {
        return await _mediator.Send(new GetClusterVariantDetailsRequest(variant));
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{type}")]
    public async Task<IEnumerable<ClusterVariantPreview>> GetVariantPreviewsOfType([FromRoute] string type)
    {
        return await _mediator.Send(new GetClusterVariantPreviewsRequest(type));
    }

    [HttpPost]
    [Route("/api/[controller]/[action]")]
    public async Task<IActionResult> SaveAllClusterVariants()
    {
        await _mediator.Send(new CreateClusterDetailsRequest());
        return Ok();
    }

    [HttpPost]
    [Route("/api/[controller]/[action]")]
    public async Task<IActionResult> UpdateAllClusterPreviews()
    {
        await _mediator.Send(new UpdateClusterPreviewsRequest());
        return Ok();
    }

    [HttpPost]
    [Route("/api/[controller]/[action]")]
    public async Task<IActionResult> UpdateCurrencyPrices()
    {
        await _mediator.Send(new UpdateCurrencyPricesRequest());
        return Ok();
    }
}