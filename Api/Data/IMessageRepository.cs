using Share.Model;

namespace View.Data;

public interface IMessageRepository
{
    Task<List<Message>> GetMessages(int pageNumber, int pageSize);
    Task<Message> AddMessage(Message message);
}