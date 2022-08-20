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

    public OnlineUsersController(IConfiguration configuration, IConnectionMultiplexer redis)
    {
        _server = redis.GetServer(configuration["Redis:ConnectionString"]);
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
                Id = userId,
                Username = userName
            };

            users.Add(user);
        }
        return users;
    }
}
