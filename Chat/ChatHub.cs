using Microsoft.AspNetCore.SignalR;
using Share;
using Share.Model;
using Share.Util;

namespace Chat;

public class ChatHub(ILogger<ChatHub> logger) : Hub<IChatClient>
{

    public async Task SendMessage(Message message)
    {
		try
		{
			var cleanContent = InputSanitizer.Clean(message.Content);
			var cleanMessage = MessageFactory.Create(message.Sender, cleanContent);
			await Clients.All.ReceiveMessage(cleanMessage);
			logger.LogInformation("Sending message: '{}' from user: '{}'", cleanMessage.Content, cleanMessage.Sender); 
		}
		catch (Exception e)
		{
			logger.LogError("Error sending message: {}", e);
		}
    }

    public override async Task OnConnectedAsync()
    {
		try
		{
			logger.LogInformation("Client: '{}', connecting to ChatHub", Context.ConnectionId);
			await base.OnConnectedAsync();
		}
		catch (Exception e)
		{
			logger.LogError("Error connecting client: '{}' to ChatHub -- Error: {}", Context.ConnectionId, e);
		}
    }

    public override async Task OnDisconnectedAsync(Exception? e)
    {
		if (e != null)
		{
			logger.LogError("Connection {} disconnected in error: {}", Context.ConnectionId, e);
		}
		await base.OnDisconnectedAsync(e);
    }

}
