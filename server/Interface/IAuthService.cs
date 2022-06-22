using server.Dto.RequestDto;
using server.Dto.ResponseDto;
using server.Model.User;

namespace server.Interface
{
    public interface IAuthService
    {
        Task signupUser(User user);
        Task<LoginResponseDto> loginUser(LoginRequestDto loginRequestDto);
        Task<bool> checkIfUsernameExists(string username);
        Task<bool> checkIfPasswordMatches(string username, string password);
    }
}
