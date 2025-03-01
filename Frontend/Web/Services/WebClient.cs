using Share;
using Share.Model;

namespace Frontend.Web.Services;
public class WebClient(string url, string username, ILogger logger) : Client(url, username, logger), IClient
{
    public List<Message> MessageList = [];
    // public event Action? OnMessageReceived;

    public override Task ReceiveMessage(Message message)
    {
        MessageList.Add(message);
	    // OnMessageReceived?.Invoke();
        Logger.LogInformation("Message received: {}", message.Content);
	    return Task.CompletedTask;
    }
    
    public override List<Message> GetMessageList() => MessageList;
    
}
