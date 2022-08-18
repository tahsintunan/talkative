using GetOnlineUsers.Dto;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace GetOnlineUsers.Controllers;


[ApiController]
[Route("[controller]")]
public class OnlineUsersController : ControllerBase
{
    private readonly IServer _server;
    private readonly IDatabase _database;
    
    public OnlineUsersController()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var option = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            ConnectTimeout = 30000,
            Ssl = false,
            Password = configuration["Redis:Password"],
            EndPoints = { configuration["Redis:ConnectionString"] }
        };
        var redis = ConnectionMultiplexer.Connect(option);
        _server = redis.GetServer(option.EndPoints.First());
        _database = redis.GetDatabase();
    }

    
    [HttpGet(Name = "OnlineUsers")]
    public List<UserDto> Get()
    {
        const string pattern = "kernel-panic*";
        var users = new List<UserDto>();
        
        foreach (var key in _server.Keys(pattern: pattern))
        {
            var userId = key.ToString()[13..];
            var userName = _database.StringGet(key);
            
            var user = new UserDto
            {
                UserId = userId,
                UserName = userName
            };
            
            users.Add(user);
        }
        return users;
    }
}
