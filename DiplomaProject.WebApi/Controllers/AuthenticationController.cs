using DiplomaProject.Application.DTOs.Authentication;
using DiplomaProject.Application.UseCases.Authentication.Commands;
using DiplomaProject.Application.UseCases.Authentication.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaProject.WebApi.Controllers;

public class AuthenticationController(IMediator mediator, IConfiguration configuration)
    : BaseController(mediator)
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthUserDto loginUser)
    {
        Response.Cookies.Delete("authorization", new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential = true,
            Path = "/"
        });

        var tokenModelResponse = await _mediator.Send(new LoginUserCommand(loginUser));
        // var tokenExpiration = int.Parse(configuration["Jwt:AccessTokenValidityInHours"]);

        Response.Cookies.Append("authorization", tokenModelResponse.Data.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.Now.AddSeconds(tokenModelResponse.Data.AccessTokenExpiresIn),
            Path = "/"
        });

        Response.Cookies.Append("refreshToken", tokenModelResponse.Data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.Now.AddSeconds(tokenModelResponse.Data.RefreshTokenExpiresIn),
            Path = "/"
        });

        return Ok();
    }

    [HttpPatch("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized();
        }

        var tokenModelResponse = await _mediator.Send(new RefreshTokenCommand(refreshToken));

        Response.Cookies.Append("authorization", tokenModelResponse.Data.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.Now.AddSeconds(tokenModelResponse.Data.AccessTokenExpiresIn),
            Path = "/"
        });

        Response.Cookies.Append("refreshToken", tokenModelResponse.Data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential = true,
            Expires = DateTime.Now.AddSeconds(tokenModelResponse.Data.RefreshTokenExpiresIn),
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

    [HttpPatch("verifyEmail")]
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        return Ok(await _mediator.Send(new VerifyEmailCommand(token, email)));
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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("isAuthenticated")]
    public Task<IActionResult> IsAuthenticated()
    {
        return Task.FromResult<IActionResult>(Ok());
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser()
    {
        return Ok(await _mediator.Send(new GetUserQuery()));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("deleteUserSession")]
    public async Task<IActionResult> DeleteUserSession(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteUserSessionCommand(id)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("userSessions")]
    public async Task<IActionResult> GetUserSessions()
    {
        return Ok(await _mediator.Send(new GetUserSessionsQuery()));
    }
}