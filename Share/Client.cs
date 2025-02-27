using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Share.Model;

namespace Share;

public abstract class Client
{
    private readonly HubConnection 	Connection;
    private readonly ILogger 		_logger;
    private readonly string 		_url;

    public string Username { get; }

    public Client(string url, string username, ILogger logger)
    {
        Username = username;
        _logger = logger;
	_url = url;
        Connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        Connection.On<Message>("ReceiveMessage", ReceiveMessage);
        Connection.Closed       += e => OnConnectionClosed(e);
        Connection.Reconnecting += e => OnReconnecting(e);
        Connection.Reconnected  += e => OnReconnected(e);
    }


    public async Task SendMessage(Message M)
    {
        try 
	{ 
	    await Connection.InvokeAsync("SendMessage", M); 
	}
        catch (Exception e) 
	{ 
	    _logger.LogError("Error sending message: {} -- Error: {}", M.Content, e); 
	}
    }

    protected virtual Task OnConnectionClosed(Exception? e)
	{
		_logger.LogError("Client '{}' connection to {} closed - Error: {}", Username, _url, e);
		return Task.CompletedTask;
	}

    protected virtual Task OnReconnecting(Exception? e)
	{
		_logger.LogError("Client '{}' reconnecting to {} - Error: {}", Username, _url, e);
		return Task.CompletedTask;
	}

    protected virtual Task OnReconnected(string? e)
	{
		_logger.LogInformation("Client '{}' reconnected to {} - Connection Id: {}", Username, _url, e);
		return Task.CompletedTask;
	}

    public async Task StartConnection()
    {
	    try 
        { 
            await Connection.StartAsync();
            _logger.LogInformation("Client '{}' connected to {}", Username, _url);
        }
	    catch (Exception e) { 
            _logger.LogError("Client '{}' failed to connect to {} - Error: {e}", Username, _url, e);
        }
    }

    public abstract Task ReceiveMessage(Message M);

}
