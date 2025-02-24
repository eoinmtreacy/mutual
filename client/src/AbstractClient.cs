using Microsoft.AspNetCore.SignalR.Client;
using Model;

namespace Client;

public abstract class AbstractClient
{
    public string Username { get; }

    private readonly HubConnection 	_connection;

    public AbstractClient(string username)
    {
	Username = username;
	_connection = new HubConnectionBuilder()
	    .WithUrl("http://localhost:5250/chat")
	    .WithAutomaticReconnect()
	    .Build();
	_connection.On<Message>("ReceiveMessage", ReceiveMessage);
	StartConnection();
    }

    public abstract void ReceiveMessage(Message m);

    private void StartConnection()
    {
	    try { _connection.StartAsync(); }
	    catch (Exception e) { Console.WriteLine($"Error connecting: {e}"); }
    }

    public async void SendMessage(Message m)
    {
        try { await _connection.InvokeAsync("SendMessage", m); }
        catch { Console.WriteLine($"Error sending message: {m.Content}"); }
    }
}
