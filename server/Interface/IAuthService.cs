using server.Dto.RequestDto.LoginRequestDto;
using server.Dto.RequestDto.SignupRequestDto;
using server.Dto.ResponseDto;

namespace server.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDto> signupUser(SignupRequestDto signupRequestDto);
        Task<AuthResponseDto> loginUser(LoginRequestDto loginRequestDto);
        Task<bool> checkIfUsernameExists(string username);
        Task<bool> checkIfPasswordMatches(string username, string password);
    }
}
