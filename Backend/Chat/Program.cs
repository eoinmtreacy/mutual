using Chat;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load(builder.Environment.IsDevelopment() ? "../../.dev.env" : "../../.prod.env");
const string myAllowedOrigins = "myAllowedOrigins";
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        myAllowedOrigins,
        policy =>
        {
            policy
                .WithOrigins(
                    Environment.GetEnvironmentVariable("FRONTEND_WEB") ?? string.Empty,
                    Environment.GetEnvironmentVariable("ANDROID_WEB") ?? string.Empty
                    )
                .WithMethods("GET", "POST")
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5002);
    });
}
var app = builder.Build();
app.UseCors(myAllowedOrigins);
app.MapHub<ChatHub>("/chat");
app.UseHttpsRedirection();
app.Run();
