using Microsoft.AspNetCore.SignalR;
using Model;

public class ChatHub : Hub
{
    private readonly ILogger _logger;

    public ChatHub(ILogger<ChatHub> logger) 
    {
	_logger = logger;
    }
    public async Task SendMessage(string user, string message)
    {
	try
	{
	    await Clients.All.SendAsync("ReceiveMessage", user, message);
	    _logger.LogInformation("Sending message: '{}' from user: '{}'", message, user);
	}
	catch (Exception e)
	{
	    _logger.LogError("Error sending message: '{}' from user: '{}' -- {}", message, user, e);
	}
    }

    public override Task OnConnectedAsync()
    {
	try
	{
	    _logger.LogInformation("Client: '{}', connecting to ChatHub", Context.ConnectionId);
	    return base.OnConnectedAsync();
	}
	catch (Exception e)
	{
	    _logger.LogError("Error connecting client: '{}' to ChatHub", Context.ConnectionId, e);
	    return Task.CompletedTask;
	}
    }

}
