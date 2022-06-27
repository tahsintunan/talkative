namespace server.Dto.ResponseDto
{
    public class AuthResponseDto
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }

        public AuthResponseDto(string accessToken, string refreshToken)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
    }
}
