using Microsoft.AspNetCore.SignalR;
using Share.Model;

public class ChatHub : Hub
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
	    await Clients.All.SendAsync("ReceiveMessage", M.Sender, M.Content);
	    _logger.LogInformation("Sending M.Content: '{}' from M.Sender: '{}'", M.Content, M.Sender);
	}
	catch (Exception e)
	{
	    _logger.LogError("Error sending M.Content: '{}' from M.Sender: '{}' -- {}", M.Content, M.Sender, e);
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
	    _logger.LogError("Error connecting client: '{}' to ChatHub -- Error: {}", Context.ConnectionId, e);
	    return Task.CompletedTask;
	}
    }

}
