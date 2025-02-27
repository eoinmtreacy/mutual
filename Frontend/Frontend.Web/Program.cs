using Frontend.Web.Components;
using Frontend.Shared.Services;
using Frontend.Web.Services;
using Share;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add device-specific services used by the Frontend.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddScoped<WebClient>(provider =>
{
    ILogger logger = provider.GetRequiredService<ILogger<WebClient>>();
    return new WebClient("https://localhost:7088/chat", "eoin", logger);
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
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Frontend.Shared._Imports).Assembly);

app.Run();
