using Domain.Entities;

namespace Application.Interface;

public interface IChatHub
{
    public Task SendMessage(Message message);
}