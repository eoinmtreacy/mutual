using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5250/chat")
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) => receiveMessage(user, message));

static void receiveMessage(string user, string message)
{
    Console.WriteLine($"User: {user} Message: {message}");
}

try { await connection.StartAsync(); }
catch (Exception e) { Console.WriteLine($"Error connecting: {e}"); }

Task.Run(async () =>
{
    while (true)
    {
        Console.Write(">> ");
        string message = Console.ReadLine();
        try { await connection.InvokeAsync("SendMessage", "User", message); }
        catch { Console.WriteLine($"Error sending message: {message}"); }
    }
});

await Task.Delay(-1);
