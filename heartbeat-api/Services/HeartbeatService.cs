using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using heartbeat_api.Interfaces;


namespace heartbeat_api.Services
{
    public class HeartbeatService: IHeartbeatService
    {
        private readonly IDatabase _db;
        public HeartbeatService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        

        public async Task Heartbeat(string userId, string userName, string prefix, int expiry)
        {
            var key = prefix + userId;
            var value = userName;
            await _db.StringSetAsync(key, value, TimeSpan.FromSeconds(expiry));
        }


        public bool IsValidRequest(HttpRequest request)
        {
            if (!request.Cookies.ContainsKey("authorization"))
            {
                return false;
            }
            var authHeader = request.Cookies["authorization"];
            if (authHeader == null || !authHeader.StartsWith("Bearer ") || authHeader.Split(' ').Length != 2)
            {
                return false;
            }
            var token = authHeader.Split(' ')[1];
            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.CanReadToken(token);
        }


        public string GetToken(HttpRequest request)
        {
            var authHeader = request.Cookies["authorization"];
            var token = authHeader!.Split(' ')[1];
            return token;
        }


        public string GetUserId(string token)
        {
            const string claimType = "user_id";
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            var userId = jwtToken.Claims.First(claim => claim.Type == claimType).Value;
            return userId;
        }

        
        public string GetUserName(string token)
        {
            const string claimType = "unique_name";
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            var userName = jwtToken.Claims.First(claim => claim.Type == claimType).Value;
            return userName;
        }
        
    }
}