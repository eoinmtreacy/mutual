using Frontend.Shared.Services;
using Share.Model;

namespace Frontend.Web.Services;
public class WebClient(string url, string username, ILogger logger) : Client(url, username, logger) 
{
    protected override void ProcessMessage(Message message)
    {
        MessageList.Add(message);
    }
    
}
