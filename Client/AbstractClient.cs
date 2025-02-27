using Microsoft.AspNetCore.SignalR.Client;
using Model;

namespace Client;

public abstract class AbstractClient
{
    private readonly HubConnection Connection;
    private readonly ILogger _logger;
    public string Username { get; }


    public AbstractClient(string username, ILogger logger)
    {
        Username = username;
        _logger = logger;
        Connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7088/chat")
            .WithAutomaticReconnect()
            .Build();
        Connection.On<Message>("ReceiveMessage", ReceiveMessage);
        Connection.Closed       += e => OnConnectionClosed(e);
        Connection.Reconnecting += e => OnReconnecting(e);
        Connection.Reconnected  += e => OnReconnected(e);
    }


    public async Task SendMessage(Message m)
    {
        try { await Connection.InvokeAsync("SendMessage", m); }
        catch { Console.WriteLine($"Error sending message: {m.Content}"); }
    }

    protected virtual Task OnConnectionClosed(Exception? e)
	{
		_logger.LogError("Client '{Username}' connection closed - Error: {e}", Username, e);
		return Task.CompletedTask;
	}

    protected virtual Task OnReconnecting(Exception? e)
	{
		_logger.LogError("Client '{Username}' reconnecting - Error: {e}", Username, e);
		return Task.CompletedTask;
	}

    protected virtual Task OnReconnected(string? e)
	{
		_logger.LogInformation("Client '{Username}' reconnected - Connection Id: {e}", Username, e);
		return Task.CompletedTask;
	}
    public async Task StartConnection()
    {
	    try 
        { 
            await Connection.StartAsync();
            _logger.LogInformation("Client '{Username}' connected", Username);
        }
	    catch (Exception e) { 
            _logger.LogError("Client '{Username}' failed to connect - Error: {e}", Username, e);
        }
    }

    protected abstract void ReceiveMessage(Message m);

}
