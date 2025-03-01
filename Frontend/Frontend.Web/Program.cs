using Frontend.Web.Components;
using Frontend.Shared.Services;
using Frontend.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped<Client, WebClient>(provider =>
{
    ILogger logger = provider.GetRequiredService<ILogger<WebClient>>();
    return new WebClient("https://localhost:7088/chat", "eoin", logger);
});

var app = builder.Build();

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
