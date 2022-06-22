using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using server.Interface;
using server.Model;
using server.Model.User;
using System.Security.Cryptography;
using System.Text;
using server.Dto.RequestDto;
using server.Dto.ResponseDto;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<RefreshToken> _refreshTokenCollection;
        private readonly IConfiguration _configuration;
        public AuthService(
            IConfiguration configuration,
            IOptions<UserDatabaseConfig> userDatabaseConfig)
        {
            _configuration = configuration;

            var mongoClient = new MongoClient(
            userDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userDatabaseConfig.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.Value.UserCollectionName);

            _refreshTokenCollection = mongoDatabase.GetCollection<RefreshToken>(
                userDatabaseConfig.Value.RefreshTokenCollectionName);
        }

        public async Task signupUser(User user)
        {
            if (user == null || user.Username == null || user.Password == null)
                throw new ArgumentNullException("Invalid request");
            
            string hashedPassword;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, user.Password);
            }
            User newUser = new User()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = user.Username,
                Password = hashedPassword,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth.Date
            };
            await _userCollection.InsertOneAsync(newUser);
        }

        public async Task<LoginResponseDto> loginUser(LoginRequestDto request)
        {
            if (request == null || request.Username == null || request.Password == null)
                throw new ArgumentNullException("Invalid request");
            
            var user = await _userCollection.Find(user => user.Username == request.Username).FirstOrDefaultAsync();
            string accessToken = GenerateAccessToken(user);     // generate access token
            string refreshToken = await GetRefreshToken(user);  // get refresh token
            LoginResponseDto loginResponse = new LoginResponseDto(accessToken, refreshToken);
            return loginResponse;
        }

        public async Task<bool> checkIfUsernameExists(string username)
        {
            var foundUser = await _userCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
            if (foundUser == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> checkIfPasswordMatches(string username, string password)
        {
            var user = await _userCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
            string hashedPassword;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, password);
            }
            return hashedPassword == user.Password;
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string password)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private string GenerateAccessToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException("Invalid request");
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:AccessTokenKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }

        private async Task<string> GetRefreshToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException("Invalid request");

            var refreshToken = await _refreshTokenCollection.Find(token => token.UserId == user.Id).FirstOrDefaultAsync();
            if (refreshToken == null || refreshToken.ExpiresAt < DateTime.Now)
            {
                RefreshToken newRefreshToken = new RefreshToken()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserId = user.Id,
                    Token = GenerateRefreshToken(),
                    ExpiresAt = DateTime.Now.AddDays(7),
                    CreatedAt = DateTime.Now
                };
                await _refreshTokenCollection.InsertOneAsync(newRefreshToken);
                refreshToken = newRefreshToken;
            }
            return refreshToken.Token!;
        }

        private string GenerateRefreshToken()
        {
            var randomString = new byte[128];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomString);
            return Convert.ToHexString(randomString);
        }
    }
}
