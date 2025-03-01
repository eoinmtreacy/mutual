using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Share.Model;

namespace Share;

public abstract class Client : IClient
{
    public string Username { get; }
    
    private readonly string _url;
    private readonly HubConnection _connection;
    private readonly ILogger _logger;

    public event Action? OnMessageReceived;

    protected Client(string url, string username, ILogger logger)
    {
        Username = username;
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
        _logger.LogError("Client '{}' connection to {} closed - Error: {}", Username, _url, e);
        return Task.CompletedTask;
    }

    private Task OnReconnecting(Exception? e)
    {
        _logger.LogError("Client '{}' reconnecting to {} - Error: {}", Username, _url, e);
        return Task.CompletedTask;
    }

    private Task OnReconnected(string? e)
    {
        _logger.LogInformation(
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
            await _connection.StartAsync();
            _logger.LogInformation("Client '{}' connected to {}", Username, _url);
        }
        catch (Exception e)
        {
            _logger.LogError("Client '{}' failed to connect to {} - Error: {e}", Username, _url, e);
        }
    }

    public bool IsConnected() => _connection.State == HubConnectionState.Connected;

    public Task ReceiveMessage(Message message)
    {
        ProcessMessage(message);
        OnMessageReceived?.Invoke();
        _logger.LogInformation("Client '{}' received message from {}", Username, _url);
        return Task.CompletedTask;
    }
    
    public abstract void ProcessMessage(Message message);
    
    public abstract List<Message> GetMessageList();
}
