using server.Model.Message;

namespace server.Interface;

public interface IChatHub
{
    public Task SendMessage(Message message);
}