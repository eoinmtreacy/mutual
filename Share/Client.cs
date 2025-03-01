using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Share.Model;

namespace Share;

public abstract class Client
{
    private readonly string _url;
    public ILogger Logger { get; }

    public HubConnection Connection { get; }
    public string Username { get; }

    public Client(string url, string username, ILogger logger)
    {
        Username = username;
        Logger = logger;
        _url = url;
        Connection = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();
        Connection.On<Message>("ReceiveMessage", ReceiveMessage);
        Connection.Closed += e => OnConnectionClosed(e);
        Connection.Reconnecting += e => OnReconnecting(e);
        Connection.Reconnected += e => OnReconnected(e);
    }

    public async Task SendMessage(Message message)
    {
        try
        {
            await Connection.InvokeAsync("SendMessage", message);
        }
        catch (Exception e)
        {
            Logger.LogError("Error sending message: {} -- Error: {}", message.Content, e);
        }
    }
    public Task OnConnectionClosed(Exception? e)
    {
        Logger.LogError("Client '{}' connection to {} closed - Error: {}", Username, _url, e);
        return Task.CompletedTask;
    }

    public virtual Task OnReconnecting(Exception? e)
    {
        Logger.LogError("Client '{}' reconnecting to {} - Error: {}", Username, _url, e);
        return Task.CompletedTask;
    }

    public virtual Task OnReconnected(string? e)
    {
        Logger.LogInformation(
            "Client '{}' reconnected to {} - Connection Id: {}",
            Username,
            _url,
            e
        );
        return Task.CompletedTask;
    }

    public async Task StartConnection()
    {
        try
        {
            await Connection.StartAsync();
            Logger.LogInformation("Client '{}' connected to {}", Username, _url);
        }
        catch (Exception e)
        {
            Logger.LogError("Client '{}' failed to connect to {} - Error: {e}", Username, _url, e);
        }
    }

    public bool IsConnected() => Connection.State == HubConnectionState.Connected;
    
    public abstract Task ReceiveMessage(Message m);
    
    public abstract List<Message> GetMessageList();
}
