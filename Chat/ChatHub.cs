using Microsoft.AspNetCore.SignalR;
using Model;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine(user + " " + message);
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override Task OnConnectedAsync()
    {
	Console.WriteLine($" Client: {Context.ConnectionId}, connected to ChatHub");
	return base.OnConnectedAsync();
    }

}
