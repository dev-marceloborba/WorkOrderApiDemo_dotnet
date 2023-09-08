using Microsoft.AspNetCore.Mvc;
using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Data;
using WorkOrderApi.Handlers;

namespace WorkOrderApi.Controllers;

[ApiController]
[Route("v1/work-orders")]
public class WorkOrderController : ControllerBase
{
    [Route("")]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateWorkOrderRequest command,
        [FromServices] ICreateWorkOrderHandler handler
    )
    {
        var response = await handler.Handle(command);
        return Ok(response);
    }

    [Route("{id}")]
    [HttpPut]
    public async Task<ActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateWorkOrderRequest command,
        [FromServices] IUpdateWorkOrderHandler handler
    )
    {
        var response = await handler.Handle(command);
        return Ok(response);
    }

    [Route("{id}")]
    [HttpDelete]
    public async Task<ActionResult> Delete(
        [FromRoute] int id
    )
    {
        return Ok();
    }

    [Route("{id}")]
    [HttpGet]
    public async Task<ActionResult> FindById(
        [FromRoute] int id,
        [FromBody] FindWorkOrderByIdRequest command,
        [FromServices] IFindWorkOrderByIdHandler handler
    )
    {
        var result = await handler.Handle(command);
        return Ok(result);
    }

    [Route("")]
    [HttpGet]
    public async Task<ActionResult> FindAll(
        [FromServices] WorkOrderRepository repository
    )
    {
        var result = await repository.FindAllAsync();
        return Ok(result);
    }
}