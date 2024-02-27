namespace DiplomaProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;
}