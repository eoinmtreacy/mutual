using Model;
using Client;

class WebAppClient(string username, ILogger logger) : AbstractClient(username, logger)
{
    public List<Message> MessageList { get; set; } = [];

    protected override void ReceiveMessage(Message M)
    {
        MessageList.Add(M);
    }
}
