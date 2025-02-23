using Microsoft.AspNetCore.SignalR.Client;

namespace mutual.client;

class Client
{
    public  string		Username { get; }

    private HubConnection 	_connection;

    public Client(string username)
    {
	Username = username;
	_connection = new HubConnectionBuilder()
	    .WithUrl("http://localhost:5250/chat")
	    .WithAutomaticReconnect()
	    .Build();
	_connection.On<string,string>(
		"ReceiveMessage", (user, message) 
		    => ReceiveMessage(user, message));
	StartConnection();
    }

    private void ReceiveMessage(string user, string message)
    {
	Console.WriteLine($"User: {user} Message: {message}");
    }

    private void StartConnection()
    {
	try { _connection.StartAsync(); }
	catch (Exception e) { Console.WriteLine($"Error connecting: {e}"); }
    }

    public void SendMessage(string message)
    {
        try { _connection.InvokeAsync("SendMessage", Username, message); }
        catch { Console.WriteLine($"Error sending message: {message}"); }
    }
}
