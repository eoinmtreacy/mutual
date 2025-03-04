using System.Net.Http.Json;
using Frontend.Shared.Services;
using Share.Model;

namespace Frontend.Services;

public class MessageServiceApp(string url) : IMessageService
{
    private readonly HttpClient _httpClient =  new () { BaseAddress = new Uri(url) };
    
    public void AddMessage(Message message)
    {
        _httpClient.PostAsJsonAsync("/messages", message);
    }

    public List<Message> GetMessages()
    {
        return _httpClient.GetFromJsonAsync<List<Message>>($"/messages/").GetAwaiter().GetResult() ?? [];
    }
}