namespace Application.Common.Interface
{
    public interface IToken
    {
        string GenerateAccessToken(
            string id,
            string username,
            string email,
            string role,
            int expireDays
        );

        string? ValidateAccessToken(string token);
    }
}
