namespace Share;
using Model;

public interface IChatClient
{
   public Task ReceiveMessage(Message message);
   
}