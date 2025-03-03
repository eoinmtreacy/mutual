using Frontend.Web.Components;
using Frontend.Shared.Services;
using Frontend.Web.Services;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"] ?? string.Empty;
        options.ClientId = builder.Configuration["Auth0:ClientId"] ?? string.Empty;
    });

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped<Client, ClientWeb>(provider =>
{
    ILogger logger = provider.GetRequiredService<ILogger<ClientWeb>>();
    return new ClientWeb(builder.Configuration["ServiceUrls:Web:Chat"] ?? "", logger);
});

builder.Services.AddScoped<IMessageService, MessageServiceWeb>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new MessageServiceWeb(configuration["ServiceUrls:Web:Api"] ?? "");
});

var app = builder.Build();

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
