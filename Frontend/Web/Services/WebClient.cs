using Share;
using Share.Model;

namespace Frontend.Web.Services;
public class WebClient(string url, string username, ILogger logger) : Client(url, username, logger), IClient
{
    public List<Message> MessageList = [];

    public override void ProcessMessage(Message message)
    {
        MessageList.Add(message);
    }

    public override List<Message> GetMessageList() => MessageList;
    
}
