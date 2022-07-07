using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace GetOnlineUsers.Controllers;


[ApiController]
[Route("[controller]")]
public class OnlineUsersController : ControllerBase
{
    private readonly ILogger<OnlineUsersController> _logger;
    private readonly IConfiguration _configuration;
    public OnlineUsersController(ILogger<OnlineUsersController> logger)
    {
        _logger = logger;
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    [HttpGet(Name = "OnlineUsers")]
    public List<string> Get()
    {
        string pattern = "kernel-panic:*";

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_configuration.GetSection("RedisConnectionString").Value);
        var server = redis.GetServer(_configuration.GetSection("RedisConnectionString").Value);
        IDatabase db = redis.GetDatabase();

        var users = new List<string>();
        foreach (var key in server.Keys(pattern: pattern))
        {
            var userId = db.StringGet(key);
            users.Add(userId!);
        }
        return users;
    }
}

