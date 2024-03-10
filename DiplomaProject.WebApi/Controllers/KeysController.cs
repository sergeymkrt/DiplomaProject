using DiplomaProject.Application.DTOs.Keys;
using DiplomaProject.Application.UseCases.Keys.Commands;
using DiplomaProject.Application.UseCases.Keys.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaProject.WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class KeysController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateKey([FromBody] KeyDto dto)
    {
        return Ok(await _mediator.Send(new CreateKeyCommand(dto)));
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateKey([FromBody] KeyDto dto)
    {
        return Ok(await _mediator.Send(new UpdateKeyCommand(dto)));
    }
    //
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteKey(long id)
    // {
    //     return Ok(await _mediator.Send(new DeleteKeyCommand(id)));
    // }

    [HttpGet]
    public async Task<IActionResult> GetKeys(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string search,
        [FromQuery] string orderByColumn,
        [FromQuery] bool isAsc)
    {
        return Ok(await _mediator.Send(new GetKeysQuery(pageNumber, pageSize, search, orderByColumn, isAsc)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetKey(long id)
    {
        return Ok(await _mediator.Send(new GetKeyByIdQuery(id)));
    }
}