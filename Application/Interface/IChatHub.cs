using server.Domain.Entities;

namespace server.Application.Interface;

public interface IChatHub
{
    public Task SendMessage(Message message);
}