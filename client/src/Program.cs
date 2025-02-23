using mutual.client;

Client c = new(args[0]);

await Task.Run(() =>
{
    while (true)
    {
        Console.Write(">> ");
	string? message;
	do { message = Console.ReadLine(); } 
	while (message == null);
        try { c.SendMessage(message); }
        catch { Console.WriteLine($"Error sending message: {message}"); }
    }
});

await Task.Delay(-1);

