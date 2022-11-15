using System.Net.Http.Headers;
using Application.Auth.Commands.Login;
using Application.Auth.Commands.Signup;
using Application.Users.Commands.ForgetPassword;
using Application.Users.Commands.ResetPassword;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupCommand signupCommand)
    {
        await Mediator.Send(signupCommand);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        var accessToken = await Mediator.Send(request);

        var value = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpContext.Response.Cookies.Append(
            "authorization",
            value.ToString(),
            new CookieOptions { HttpOnly = false, Expires = DateTime.Now.AddDays(7) }
        );

        return NoContent();
    }

    [HttpPost("forget-password")]
    public async Task<ActionResult> ForgotPassword(ForgetPasswordCommand forgetPasswordCommand)
    {
        await Mediator.Send(forgetPasswordCommand);
        return NoContent();
    }

    [HttpGet("reset-password/{token}")]
    public async Task<ActionResult> ResetPassword(string token)
    {
        await Mediator.Send(new ResetPasswordCommand { Token = token });
        return Ok("Your new password has been sent via email");
    }

    [HttpPost("logout")]
    public Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("authorization");
        return Task.FromResult<IActionResult>(
            new OkObjectResult(new { message = "User logged out" })
        );
    }
}
