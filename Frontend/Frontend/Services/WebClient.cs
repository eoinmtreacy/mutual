using Frontend.Shared.Services;
using Microsoft.Extensions.Logging;
using Share.Model;

namespace Frontend.Services;
public class WebClient(string url, ILogger logger) : Client(url, logger) {}
