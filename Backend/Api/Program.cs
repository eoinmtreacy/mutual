using Share.Model;
using View.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load(builder.Environment.IsDevelopment() ? "../../.dev.env" : "../../.prod.env");
const string myAllowedOrigins = "myAllowedOrigins";
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
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5003);
    });
}

var app = builder.Build();
app.UseCors(myAllowedOrigins);
app.UseHttpsRedirection();

app.MapGet("/messages/", async (IMessageRepository messageRepository) =>
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

app.MapPost("/messages", async (Message message, IMessageRepository messageRepository) =>
{
    try
    {
        await messageRepository.AddMessage(message);
        return Results.Created();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving messages: {ex.Message}");
    }
});

app.Run();
