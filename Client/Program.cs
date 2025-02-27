using Client;
using Model;

string username = args[0];

using var loggerFactory = LoggerFactory.Create(builder =>
{
	builder
		.AddConsole()
		.SetMinimumLevel(LogLevel.Debug);
});

ILogger logger = loggerFactory.CreateLogger<Program>();

ConsoleClient c = new(username, logger);
await c.StartConnection();

await Task.Run(async () =>
{
    while (true)
    {
		Console.Write(">> ");
		string? content;
		do content = Console.ReadLine();
			while (content == null);
		Message M = new(c.Username, content, DateTime.Now);
		try { await c.SendMessage(M); }
		catch { Console.WriteLine($"Error sending message: {M}"); }
    }
});

await Task.Delay(-1);

class ConsoleClient(string username, ILogger logger) : AbstractClient(username, logger)
{

	protected override void ReceiveMessage(Message m)
	{
		Console.WriteLine($"{m.Sender}");
		Console.WriteLine($"\t{m.Content}");
		Console.WriteLine($"\t{m.Timestamp}");
	}

}
