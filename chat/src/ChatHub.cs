using Microsoft.AspNetCore.SignalR;
using Mutual.Model;

public class ChatHub : Hub
{
    public async Task SendMessage(Message m)
        => await Clients.All.SendAsync("ReceiveMessage", m);

    public override Task OnConnectedAsync()
    {
	Console.WriteLine($" Client : {Context.ConnectionId}, connected to ChatHub");
	return base.OnConnectedAsync();
    }

}
