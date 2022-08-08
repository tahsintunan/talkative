using Microsoft.AspNetCore.Mvc;
using server.Dto.RequestDto.LoginRequestDto;
using server.Dto.RequestDto.SignupRequestDto;

namespace server.Interface
{
    public interface IAuthService
    {
        Task<IActionResult> SignupUser(SignupRequestDto signupRequestDto);
        Task<IActionResult> LoginUser(LoginRequestDto loginRequestDto, HttpContext httpContext);
        Task<IActionResult> LogoutUser(HttpContext httpContext);
        Task<bool> CheckIfUserExists(string username, string email);
        Task<bool> CheckIfUsernameExists(string username);
        Task<bool> CheckIfPasswordMatches(string username, string password);
    }
}
