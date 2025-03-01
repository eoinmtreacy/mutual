using Chat;

var builder = WebApplication.CreateBuilder(args);
string MyAllowedOrigins = "myAllowedOrigins";
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        MyAllowedOrigins,
        policy =>
        {
            policy
                .WithOrigins("https://localhost:7208")
                .WithMethods("GET", "POST")
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});
var app = builder.Build();
app.UseCors(MyAllowedOrigins);
app.MapHub<ChatHub>("/chat");
app.UseHttpsRedirection();
app.Run();
