using Microsoft.AspNetCore.SignalR;
using Share;
using Share.Model;
using Share.Util;

namespace Chat;

public class ChatHub(ILogger<ChatHub> logger) : Hub<IClient>
{
    private readonly ILogger _logger = logger;

    public async Task SendMessage(Message message)
    {
		try
		{
			var cleanContent = InputSanitizer.Clean(message.Content);
			var cleanMessage = message with { Content = cleanContent };
			await Clients.All.ReceiveMessage(cleanMessage);
			_logger.LogInformation("Sending message: '{}' from user: '{}'", cleanMessage.Content, cleanMessage.Sender); 
		}
		catch (Exception e)
		{
			_logger.LogError("Error sending message: {}", e);
		}
    }

    public override async Task OnConnectedAsync()
    {
		try
		{
			_logger.LogInformation("Client: '{}', connecting to ChatHub", Context.ConnectionId);
			await base.OnConnectedAsync();
		}
		catch (Exception e)
		{
			_logger.LogError("Error connecting client: '{}' to ChatHub -- Error: {}", Context.ConnectionId, e);
		}
    }

    public override async Task OnDisconnectedAsync(Exception? e)
    {
		if (e != null)
		{
			_logger.LogError("Connection {} disconnected in error: {}", Context.ConnectionId, e);
		}
		await base.OnDisconnectedAsync(e);
    }

}
