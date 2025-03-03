using Share.Model;

namespace View.Data;

public interface IMessageRepository
{
    Task<List<Message>> GetMessages();
    Task<Message> AddMessage(Message message);
}