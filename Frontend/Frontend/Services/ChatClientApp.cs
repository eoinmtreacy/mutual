using Frontend.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Frontend.Services;
public class ChatClientApp(string url, ILogger logger) : ChatClient(url, logger) {}
