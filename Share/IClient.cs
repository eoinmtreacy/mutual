namespace Share;
using Model;

public interface IClient
{
   public Task ReceiveMessage(Message message);
   
}