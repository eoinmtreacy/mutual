using Chat;
using Chat.Data;
using Share.Model;

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
                .WithOrigins("https://localhost:7208")
                .WithMethods("GET", "POST")
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
var app = builder.Build();
app.UseCors(myAllowedOrigins);
app.MapHub<ChatHub>("/chat");
app.UseHttpsRedirection();
app.MapGet("/messages", async (IMessageRepository messageRepository) =>
{
    try
    {
        var messages = await messageRepository.GetMessages();
        return Results.Ok(messages);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving messages: {ex.Message}");
    }
});
app.Run();
