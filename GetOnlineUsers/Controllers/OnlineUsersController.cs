using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace GetOnlineUsers.Controllers;


[ApiController]
[Route("[controller]")]
public class OnlineUsersController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public OnlineUsersController()
    {
        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }

    [HttpGet(Name = "OnlineUsers")]
    public List<string> Get()
    {
        const string pattern = "kernel-panic*";
        
        var option = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            ConnectTimeout = 30000,
            Ssl = false,
            Password = _configuration["Redis:Password"],
            EndPoints = { _configuration["Redis:ConnectionString"] }
        };
        
        var redis = ConnectionMultiplexer.Connect(option);
        var server = redis.GetServer(option.EndPoints.First());
        var db = redis.GetDatabase();

        var users = new List<string>();
        foreach (var key in server.Keys(pattern: pattern))
        {
            var userId = key.ToString()[13..];
            var userName = db.StringGet(key);
            
            users.Add(userId + " " + userName);
        }
        return users;
    }
}
