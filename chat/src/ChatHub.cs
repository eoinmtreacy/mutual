using Microsoft.AspNetCore.SignalR;
using Model;

public class ChatHub : Hub
{
    public async Task SendMessage(Message M)
        => await Clients.All.SendAsync("ReceiveMessage", M);

    public override Task OnConnectedAsync()
    {
	Console.WriteLine($" Client: {Context.ConnectionId}, connected to ChatHub");
	return base.OnConnectedAsync();
    }

}
