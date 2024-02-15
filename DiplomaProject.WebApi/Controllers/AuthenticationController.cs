using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.UseCases.Authentication.Commands;
using DiplomaProject.Application.UseCases.Authentication.Queries;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaProject.WebApi.Controllers;

public class AuthenticationController : BaseController
{
    private readonly IConfiguration _configuration;
    public AuthenticationController(IMediator mediator, IConfiguration configuration) : base(mediator)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthUserDto loginUser)
    {
        var token = await _mediator.Send(new LoginUserCommand(loginUser));
        var tokenExpiration = int.Parse(_configuration["Jwt:AccessTokenValidityInHours"]);
        Response.Cookies.Append("authorization", token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.Now.AddHours(tokenExpiration),
            Path = "/"
        });
        return Ok();
    }

    [HttpDelete("logout")]
    public Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("authorization", new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential = true,
            Path = "/"
        });
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO registerUser)
    {
        return Ok(await _mediator.Send(new RegisterUserCommand(registerUser)));
    }

    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        return Ok(await _mediator.Send(new DeleteUserCommand(id)));
    }

    [HttpGet("GenerateStrongPassword")]
    public async Task<IActionResult> GenerateStrongPassword()
    {
        return Ok(await _mediator.Send(new GenerateStrongPasswordQuery()));
    }

    [Authorize]
    [HttpGet("isAuthenticated")]
    public Task<IActionResult> IsAuthenticated()
    {
        return Task.FromResult<IActionResult>(Ok());
    }

    [Authorize]
    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser()
    {
        return Ok(await _mediator.Send(new GetUserQuery()));
    }
}