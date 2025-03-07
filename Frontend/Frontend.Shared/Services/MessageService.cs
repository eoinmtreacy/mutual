using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Share.Model;

namespace Frontend.Shared.Services;

public class MessageService(string url, ILogger logger) 
{
    private readonly HttpClient _httpClient =  new () { BaseAddress = new Uri(url) };
    
    public async Task AddMessage(Message message)
    {
        try
        {
            await _httpClient.PostAsJsonAsync("/messages", message);
        }
        catch (Exception e)
        {
            logger.LogError("Error sending message: {}", e);
        }
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