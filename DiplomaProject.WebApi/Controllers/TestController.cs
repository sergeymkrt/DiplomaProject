using DiplomaProject.Application.UseCases.Test.Queries;

namespace DiplomaProject.WebApi.Controllers;

public class TestController: BaseController
{
    public TestController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _mediator.Send(new EncryptionTestQuery()));
    }
}