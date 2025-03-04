using Frontend.Shared.Services;
using Share.Model;

namespace Frontend.Web.Services;

public class MessageServiceWeb(string url) : IMessageService
{
    private readonly HttpClient _httpClient =  new () { BaseAddress = new Uri(url) };
    
    public void AddMessage(Message message)
    {
        _httpClient.PostAsJsonAsync("/messages", message);
    }

    public List<Message> GetMessages(int pageNumber)
    {
        return _httpClient.GetFromJsonAsync<List<Message>>($"/messages/{pageNumber}").GetAwaiter().GetResult() ?? [];
    }
}