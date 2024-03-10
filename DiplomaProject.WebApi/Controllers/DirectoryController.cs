﻿using DiplomaProject.Application.UseCases.Directories.Commands;
using DiplomaProject.Application.UseCases.Directories.Queries;

namespace DiplomaProject.WebApi.Controllers;

public class DirectoryController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromQuery] string name, [FromQuery] long? parentId)
    {
        return Ok(await _mediator.Send(new CreateDirectoryCommand(name, parentId)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(long id)
    {
        return Ok(await _mediator.Send(new DeleteDirectoryCommand(id)));
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string search,
        [FromQuery] string orderByColumn,
        [FromQuery] bool isAsc)
    {
        return Ok(await _mediator.Send(new GetDirectoriesQuery(pageNumber, pageSize, search, orderByColumn, isAsc)));
    }
}