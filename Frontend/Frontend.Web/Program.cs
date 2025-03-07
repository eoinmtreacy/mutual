using Frontend.Web.Components;
using Frontend.Shared.Services;
using Frontend.Web.Services;
using Auth0.AspNetCore.Authentication;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices(config =>
    {
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    }
    );

Env.Load(builder.Environment.IsDevelopment() ? "../../.dev.env" : "../../.prod.env");

builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? string.Empty;
        options.ClientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID") ?? string.Empty;
    });

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ChatClient, ChatClientWeb>(provider =>
{
    ILogger logger = provider.GetRequiredService<ILogger<ChatClientWeb>>();
    return new ChatClientWeb(Environment.GetEnvironmentVariable("BACKEND_CHAT_WEB") ?? string.Empty, logger);
});

builder.Services.AddScoped<MessageService>(provider =>
{
    ILogger logger = provider.GetRequiredService<ILogger<MessageService>>();
    return new MessageService(Environment.GetEnvironmentVariable("BACKEND_API_WEB") ?? string.Empty, logger);
});


if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5001);
    });
}

builder.WebHost.UseStaticWebAssets();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri(returnUrl)
        .Build();
    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
        .WithRedirectUri("/")
        .Build();
    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Frontend.Shared._Imports).Assembly);

app.Run();
