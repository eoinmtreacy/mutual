using Microsoft.AspNetCore.SignalR;
using Share;
using Share.Model;

public class ChatHub : Hub<IClient>
{
    private readonly ILogger _logger;

    public ChatHub(ILogger<ChatHub> logger) 
    {
	_logger = logger;
    }

    public async Task SendMessage(Message M)
    {
	try
	{
	    await Clients.All.ReceiveMessage(M);
	    _logger.LogInformation("Sending message: '{}' from user: '{}'", M.Content, M.Sender);
	}
	catch (Exception e)
	{
	    _logger.LogError("Error sending message: '{}' from user: '{}' -- Error: {}", M.Content, M.Sender, e);
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
