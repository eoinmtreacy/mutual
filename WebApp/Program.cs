using WebApp.Components;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<WebAppClient>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<WebAppClient>>();
    return new WebAppClient("eoin", logger);
});

builder.Services.AddSingleton<HubConnection>(sp => {
    return new HubConnectionBuilder()
        .WithUrl("https://localhost:7088/chat") // URL of the SignalR Hub
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
