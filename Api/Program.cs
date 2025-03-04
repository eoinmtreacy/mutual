using Share.Model;
using View.Data;

var builder = WebApplication.CreateBuilder(args);
const string myAllowedOrigins = "myAllowedOrigins";
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
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
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
