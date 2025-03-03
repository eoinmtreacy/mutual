using Frontend.Shared.Services;

namespace Frontend.Web.Services;
public class ClientWeb(string url, ILogger logger) : Client(url, logger) {}
