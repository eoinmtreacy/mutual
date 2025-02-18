using Microsoft.AspNetCore.Mvc.Testing;

namespace ChatServiceTest;

public class Test : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public Test(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ChatService_Root_Returns_HelloWorld()
    {
        HttpRequestMessage req = new(HttpMethod.Get, "/");
        var res = await _client.SendAsync(req);
        var content = await res.Content.ReadAsStringAsync();

        res.EnsureSuccessStatusCode();
        Assert.Equal("Hello World!", content);
    }
}
