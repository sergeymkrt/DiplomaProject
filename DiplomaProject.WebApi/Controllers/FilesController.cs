using DiplomaProject.Application.UseCases.Files.Commands;
using DiplomaProject.Application.UseCases.Files.Queries;

namespace DiplomaProject.WebApi.Controllers;

public class FilesController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromQuery] long keyId, [FromQuery] long directoryId)
    {
        return Ok(await _mediator.Send(new UploadFileCommand(file, keyId, directoryId)));
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string search,
        [FromQuery] string orderByColumn,
        [FromQuery] bool isAsc,
        [FromQuery] long? directoryId)
    {
        return Ok(await _mediator.Send(new GetFilesQuery(pageNumber, pageSize, search, orderByColumn, isAsc, directoryId)));
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadFile(long id)
    {
        return Ok(await _mediator.Send(new DownloadFileQuery(id)));
    }
}