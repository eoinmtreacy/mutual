using Microsoft.Extensions.Logging;
using Frontend.Shared.Services;
using Frontend.Services;
using Microsoft.Extensions.Configuration;

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

        builder.Services.AddSingleton<ChatClient, ChatClientApp>(provider =>
        {
            ILogger logger = provider.GetRequiredService<ILogger<ChatClientApp>>();
            string serviceUrl;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                serviceUrl = provider.GetRequiredService<IConfiguration>()["ServiceUrls:Android:Chat"] ?? "";
            }
            else
            {
                serviceUrl = provider.GetRequiredService<IConfiguration>()["ServiceUrls:Web:Chat"] ?? "";
            }
            return new ChatClientApp(serviceUrl, logger);
        });
        
        builder.Services.AddSingleton<MessageService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new MessageService(configuration["ServiceUrls:Android:Api"] ?? "");
        });


        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
