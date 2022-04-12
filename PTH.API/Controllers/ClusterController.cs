using MediatR;
using Microsoft.AspNetCore.Mvc;
using PTH.Logic;

namespace PTH.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ClusterController  : ControllerBase
{
    private readonly IMediator _mediator;

    public ClusterController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // [HttpGet]
    // public async Task<IEnumerable<ClusterDTO>> GetAllClusterVariants()
    // {
    //     var result = await _mediator.Send(new GetClustersRequest());
    //     return Ok();
    // }
    
    [HttpPost]
    public async Task<IActionResult> SaveAllClusterVariants()
    {
        var result = await _mediator.Send(new SaveClustersRequest());
        return Ok();
    }
}