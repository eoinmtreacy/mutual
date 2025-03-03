using Microsoft.Extensions.Logging;
using Frontend.Shared.Services;
using Frontend.Services;

namespace Frontend;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add device-specific services used by the Frontend.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();

        builder.Services.AddSingleton<ChatClient, ChatClientApp>(provider =>
        {
            ILogger logger = provider.GetRequiredService<ILogger<ChatClientApp>>();
            return new ChatClientApp("https://localhost:7088/chat", logger);
        });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
