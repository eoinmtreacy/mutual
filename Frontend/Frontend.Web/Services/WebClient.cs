using Frontend.Shared.Services;
using Share.Model;

namespace Frontend.Web.Services;
public class WebClient(string url, ILogger logger) : Client(url, logger) 
{
    protected override void ProcessMessage(Message message)
    {
        MessageList.Add(message);
    }
    
}
