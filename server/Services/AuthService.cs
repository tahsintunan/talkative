using System.Globalization;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using server.Interface;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using server.Configs.DbConfig;
using server.Model.User;
using server.Dto.RequestDto.SignupRequestDto;
using server.Dto.RequestDto.LoginRequestDto;

namespace server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<User> _userCollection;
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
        }

        public async Task<IActionResult> SignupUser(SignupRequestDto signupRequestDto)
        {
            string hashedPassword;
            using (var sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, signupRequestDto.Password!);
            }
            var newUser = new User()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = signupRequestDto.Username,
                Password = hashedPassword,
                Email = signupRequestDto.Email,
                DateOfBirth = signupRequestDto.DateOfBirth.Date
            };
            await _userCollection.InsertOneAsync(newUser);
            return new OkResult();
        }
        
        public async Task<IActionResult> LoginUser(LoginRequestDto loginRequestDto, HttpContext httpContext)
        {
            var user = await _userCollection.Find(user => user.Username == loginRequestDto.Username).FirstOrDefaultAsync();
            var accessToken = GenerateAccessToken(user);
            var value = new AuthenticationHeaderValue("Bearer", accessToken);

            httpContext.Response.Cookies.Append(
                "authorization",
                value.ToString(),
                new CookieOptions
                {
                    HttpOnly = false,
                    Expires = DateTime.Now.AddDays(7)
                }
            );
            return new OkResult();
        }

        public Task<IActionResult> LogoutUser(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("authorization");
            return Task.FromResult<IActionResult>(new OkObjectResult(new { message = "User logged out" }));
        }

        public async Task<bool> CheckIfUserExists(string username, string email)
        {
            var findUserByUsername = _userCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
            var findUserByEmail = _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
            
            await Task.WhenAll(findUserByUsername, findUserByEmail);
            
            var userByUsername = await findUserByUsername;
            var userByEmail = await findUserByEmail;
            
            return (userByUsername != null || userByEmail != null);
        }

        public async Task<bool> CheckIfUsernameExists(string username)
        {
            var foundUser = await _userCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
            return foundUser != null;
        }

        public async Task<bool> CheckIfPasswordMatches(string username, string password)
        {
            var user = await _userCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
            string hashedPassword;
            using (var sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, password);
            }
            return hashedPassword == user.Password;
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string password)
        {
            var data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private string GenerateAccessToken(User user)
        {
            if (user == null)
                throw new Exception("Invalid request");
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim("user_id", user.Id!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString(CultureInfo.CurrentCulture))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:AccessTokenKey").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }
    }
}
