using Client;
using Model;

ConsoleClient c = new(args[0]);

await Task.Run(async () =>
{
    while (true)
    {
		Console.Write(">> ");
		string? content;
		do content = Console.ReadLine();
			while (content == null);
		Message M = new(c.Username, content, DateTime.Now);
		try { c.SendMessage(M); }
		catch { Console.WriteLine($"Error sending message: {M}"); }
    }
});

await Task.Delay(-1);

class ConsoleClient(string username) : AbstractClient(username)
{
	public override void ReceiveMessage(Message m)
	{
		Console.WriteLine($"{m.Sender}");
		Console.WriteLine($"\t{m.Content}");
		Console.WriteLine($"\t{m.Timestamp}");
	}
}
