using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;


namespace heartbeat_api.Services
{
    public class HeartbeatService
    {
        private readonly IConfiguration _configuration;
        public HeartbeatService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task Heartbeat(string userId, string prefix, int expiry)
        {
            var redis = ConnectionMultiplexer.Connect(_configuration.GetSection("RedisConnectionString").Value);
            var db = redis.GetDatabase();

            var key = prefix + userId;
            var value = userId;

            await db.StringSetAsync(key, value, TimeSpan.FromSeconds(expiry));
        }


        public bool IsValidRequest(HttpRequest request)
        {
            if (!request.Headers.Keys.Contains("Authorization"))
            {
                return false;
            }

            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer ") || authHeader.Split(' ').Length != 2)
            {
                return false;
            }

            var token = authHeader.ToString().Split(' ')[1];
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(token))
            {
                return false;
            }

            return true;
        }


        public string GetToken(HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader!.ToString().Split(' ')[1];
            return token;
        }


        public string GetUserId(string token)
        {
            var claimType = "user_id";

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);

            var userId = jwtToken.Claims.First(claim => claim.Type == claimType).Value;
            return userId;
        }
    }
}