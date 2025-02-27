using Share.Model;

public interface IClient
{
    Task ReceiveMessage(Message M);
}