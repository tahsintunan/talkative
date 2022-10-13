using Domain.Entities;

namespace Application.Common.Interface;

public interface IChatHub
{
    public Task SendMessage(Message message);
}