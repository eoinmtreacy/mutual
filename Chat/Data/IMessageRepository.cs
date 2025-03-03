using Share.Model;

namespace Chat.Data;

public interface IMessageRepository
{
    Task<List<Message>> GetMessages();
    Task<Message> AddMessage(Message message);
}