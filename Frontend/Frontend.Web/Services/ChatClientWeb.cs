using Frontend.Shared.Services;

namespace Frontend.Web.Services;
public class ChatClientWeb(string url, ILogger logger) : ChatClient(url, logger) {}
