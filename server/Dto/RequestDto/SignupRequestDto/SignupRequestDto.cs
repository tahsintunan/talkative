namespace server.Dto.RequestDto.SignupRequestDto
{
    public class SignupRequestDto
    {
        public string? Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
