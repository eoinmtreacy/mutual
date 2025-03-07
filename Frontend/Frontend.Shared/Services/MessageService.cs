using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Share.Model;

namespace Frontend.Shared.Services;

public class MessageService(string url, ILogger logger) 
{
    private readonly HttpClient _httpClient =  new () { BaseAddress = new Uri(url) };
    
    public void AddMessage(Message message)
    {
        _httpClient.PostAsJsonAsync("/messages", message);
    }

    public List<Message> GetMessages()
    {
        try
        {
            return _httpClient.GetFromJsonAsync<List<Message>>($"/messages/").GetAwaiter().GetResult() ?? [];
        }
        catch (Exception e)
        {
            logger.LogError("Error getting messages: {}", e.Message);
            throw new Exception();
        }
    }
}