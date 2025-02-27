namespace Frontend.Shared.Services;
using Microsoft.Extensions.Logging;
using Share;
using Share.Model;

public class WebClient : Client
{
    public List<Message> MessageList;


    public WebClient(string url, string username, ILogger logger) : base(url, username, logger)
    {
	    MessageList = [];
    }
    public override Task ReceiveMessage(Message M)
    {
        MessageList.Add(M);
	    return Task.CompletedTask;
    }
}
