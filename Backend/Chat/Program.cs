using Chat;

var builder = WebApplication.CreateBuilder(args);
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
                    builder.Configuration["ServiceUrls:Web:Frontend"] ?? "",
                    builder.Configuration["ServiceUrls:Android:Frontend"] ?? ""
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
