using server.Model;

namespace server.Interface;

public interface IChatHub
{
    public Task SendMessage(Message message);
}