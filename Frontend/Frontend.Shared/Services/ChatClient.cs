using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Share;
using Share.Model;

namespace Frontend.Shared.Services;

public abstract class ChatClient : IChatClient
{
    private readonly string _url;
    private readonly HubConnection _connection;
    private readonly ILogger _logger;

    public event Action<Message>? OnMessageReceived;

    protected ChatClient(string url, ILogger logger)
    {
        _url = url;
        _logger = logger;
        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
        _connection.On<Message>("ReceiveMessage", ReceiveMessage);
        _connection.Closed += OnConnectionClosed;
        _connection.Reconnecting += OnReconnecting;
        _connection.Reconnected += OnReconnected;
    }

    public async Task SendMessage(Message message)
    {
        try
        {
            await _connection.InvokeAsync("SendMessage", message);
        }
        catch (Exception e)
        {
            _logger.LogError("Error sending message: {} -- Error: {}", message.Content, e);
        }
    }
    private Task OnConnectionClosed(Exception? e)
    {
        _logger.LogError("Connection '{}' connection to {} closed - Error: {}", _connection.ConnectionId, _url, e);
        return Task.CompletedTask;
    }

    private Task OnReconnecting(Exception? e)
    {
        _logger.LogError("Connection '{}' reconnecting to {} - Error: {}", _connection.ConnectionId, _url, e);
        return Task.CompletedTask;
    }

    private Task OnReconnected(string? e)
    {
        _logger.LogInformation(
            "Connection '{}' reconnected to {} - Connection Id: {}",
            _connection.ConnectionId,
            _url,
            e
        );
        return Task.CompletedTask;
    }

    public async Task StartConnection()
    {
        try
        {
            await _connection.StartAsync();
            _logger.LogInformation("Connection '{}' connected to {}", _connection.ConnectionId, _url);
        }
        catch (Exception e)
        {
            _logger.LogError("Connection '{}' failed to connect to {} - Error: {e}", _connection.ConnectionId, _url, e);
        }
    }

    public bool IsConnected() => _connection.State == HubConnectionState.Connected;
    
    public void Log(Exception e) => _logger.LogInformation("Connection '{}' - Error: {}", _connection.ConnectionId, e);
    
    public Task ReceiveMessage(Message message)
    {
        OnMessageReceived?.Invoke(message);
        _logger.LogInformation("Connection '{}' received message from {}", _connection.ConnectionId, _url);
        return Task.CompletedTask;
    }
    
}
