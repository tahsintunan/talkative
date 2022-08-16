namespace heartbeat_api.Interfaces;

public interface IHeartbeatService
{
    public Task Heartbeat(string userId, string prefix, int expiry);
    public bool IsValidRequest(HttpRequest request);
    public string GetToken(HttpRequest request);
    public string GetUserId(string token);
}