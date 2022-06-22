namespace server.Dto.ResponseDto
{
    public class LoginResponseDto
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }

        public LoginResponseDto(string accessToken, string refreshToken)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
    }
}
