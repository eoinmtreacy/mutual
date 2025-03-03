namespace Share.Model;

public class Message
{
    public string Id { get; private set; } 
    public string Sender { get; private set; }
    public string Content { get; private set;  }
    public DateTime Timestamp { get; private set; }

    public Message(string id, string sender, string content, DateTime timestamp)
    {
        Id = id;
        Sender = sender;
        Content = content;
        Timestamp = timestamp;
    }
}

public static class MessageFactory
{
    public static Message Create(string sender, string content)
    {
        return new Message(Guid.NewGuid().ToString(), sender, content, DateTime.Now);
    }
}
