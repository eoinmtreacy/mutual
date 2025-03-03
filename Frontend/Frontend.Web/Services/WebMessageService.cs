using Frontend.Shared.Services;
using Share.Model;

namespace Frontend.Web.Services;

public class WebMessageService : IMessageService
{
    private readonly HttpClient _httpClient =  new () { BaseAddress = new Uri("https://localhost:7158") };
    
    public void AddMessage(Message message)
    {
        _httpClient.PostAsJsonAsync("/messages", message);
    }

    public List<Message> GetMessages()
    {
        return _httpClient.GetFromJsonAsync<List<Message>>("/messages").GetAwaiter().GetResult() ?? [];
    }
}