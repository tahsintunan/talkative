using Application.Common.Exceptions;
using Application.Common.Interface;
using Domain.Entities;
using Infrastructure.DbConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuth
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IConfiguration _configuration;

        public AuthService(
            IConfiguration configuration,
            IOptions<UserDatabaseConfig> userDatabaseConfig
        )
        {
            _configuration = configuration;

            var mongoClient = new MongoClient(userDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(userDatabaseConfig.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.Value.UserCollectionName
            );
        }

        public async Task SignupUser(User user)
        {
            if (await CheckIfUserExists(user.Username!, user.Email!))
            {
                throw new BadRequestException("User already exists");
            }

            string hashedPassword;
            using (var sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, user.Password!);
            }
            user.Password = hashedPassword;
            await _userCollection.InsertOneAsync(user);
        }

        public async Task<string?> LoginUser(string username, string password)
        {
            if (!await CheckIfUsernameExists(username))
                throw new BadRequestException("username doesn't exist");

            if (!await CheckIfPasswordMatches(username, password))
                throw new BadRequestException("Password doesn't match");

            var user = await _userCollection
                .Find(
                    user =>
                        user.Username == username
                        && (user.IsBanned == null || user.IsBanned == false)
                )
                .FirstOrDefaultAsync();
            var accessToken = GenerateAccessToken(user);
            var value = new AuthenticationHeaderValue("Bearer", accessToken);
            return accessToken;
        }

        public async Task<bool> CheckIfPasswordMatches(string username, string password)
        {
            var user = await _userCollection
                .Find(user => user.Username == username)
                .FirstOrDefaultAsync();
            string hashedPassword;
            using (var sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, password);
            }
            return hashedPassword == user.Password;
        }

        public string GetHash(HashAlgorithm hashAlgorithm, string password)
        {
            var data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private async Task<bool> CheckIfUserExists(string username, string email)
        {
            var findUserByUsername = _userCollection
                .Find(user => user.Username == username)
                .FirstOrDefaultAsync();
            var findUserByEmail = _userCollection
                .Find(user => user.Email == email)
                .FirstOrDefaultAsync();

            await Task.WhenAll(findUserByUsername, findUserByEmail);

            var userByUsername = await findUserByUsername;
            var userByEmail = await findUserByEmail;

            return userByUsername != null || userByEmail != null;
        }

        private async Task<bool> CheckIfUsernameExists(string username)
        {
            var foundUser = await _userCollection
                .Find(user => user.Username == username)
                .FirstOrDefaultAsync();
            return foundUser != null;
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
                new Claim(
                    ClaimTypes.DateOfBirth,
                    user.DateOfBirth.ToString(CultureInfo.CurrentCulture)
                )
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JwtSettings:AccessTokenKey").Value
                )
            );
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
