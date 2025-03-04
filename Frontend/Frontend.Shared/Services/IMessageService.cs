using Share.Model;

namespace Frontend.Shared.Services;

public interface IMessageService
{
    public void AddMessage(Message message);
    public List<Message> GetMessages();
}