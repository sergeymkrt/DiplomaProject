using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.UseCases.Authentication.Commands;
using DiplomaProject.Application.UseCases.Authentication.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public async Task<IActionResult> Login(AuthUserDTO loginUser)
    {
        var token = await _mediator.Send(new LoginUserCommand(loginUser));
        var tokenExpiration = int.Parse(_configuration["Jwt:AccessTokenValidityInHours"]);
        Response.Cookies.Delete("authorization");
        Response.Cookies.Append("authorization", token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.Now.AddHours(tokenExpiration),
            Path = "/"
        });
        return Ok();
    }
    
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("authorization");
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthUserDTO registerUser)
    {
        return Ok(await _mediator.Send(new RegisterUserCommand(registerUser)));
    }
    
    [Authorize]
    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser()
    {
        return Ok(await _mediator.Send(new GetUserQuery()));
    }
}