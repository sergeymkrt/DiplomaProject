using DiplomaProject.Application.DTOs.Groups;
using DiplomaProject.Application.UseCases.Groups.Commands;
using DiplomaProject.Application.UseCases.Groups.Queries;

namespace DiplomaProject.WebApi.Controllers;

public class GroupController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] GroupDto dto)
    {
        return Ok(await _mediator.Send(new CreateGroupCommand(dto)));
    }

    [HttpPut("{groupId:long}/users/{userId}")]
    public async Task<IActionResult> AddUserToGroup([FromRoute] long groupId, [FromRoute] string userId, [FromQuery] int permissionId)
    {
        return Ok(await _mediator.Send(new AddUserToGroupCommand(groupId, userId, permissionId)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(long id)
    {
        return Ok(await _mediator.Send(new DeleteGroupCommand(id)));
    }

    [HttpDelete("{groupId:long}/users/{userId}")]
    public async Task<IActionResult> DeleteUserFromGroup([FromRoute] long groupId, [FromRoute] string userId)
    {
        return Ok(await _mediator.Send(new DeleteUserFromGroupCommand(groupId, userId)));
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string search,
        [FromQuery] string orderByColumn,
        [FromQuery] bool isAsc)
    {
        return Ok(await _mediator.Send(new GetGroupsQuery(pageNumber, pageSize, search, orderByColumn, isAsc)));
    }
}