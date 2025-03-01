using Microsoft.AspNetCore.SignalR;
using Share;
using Share.Model;

namespace Chat;

public class ChatHub(ILogger<ChatHub> logger) : Hub<IClient>
{
    private readonly ILogger _logger = logger;

    public async Task SendMessage(Message message)
    {
		try
		{
			await Clients.All.ReceiveMessage(message);
			_logger.LogInformation("Sending message: '{}' from user: '{}'", message.Content, message.Sender); 
		}
		catch (Exception e)
		{
			_logger.LogError("Error sending message: '{}' from user: '{}' -- Error: {}", message.Content, message.Sender, e);
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
