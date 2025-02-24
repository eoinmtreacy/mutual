var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o => 
	{
	    o.AddPolicy("AllowSpecificOrigin",
		    builder => 
		    {
			builder.WithOrigins("http://localhost:5134")
			    .AllowAnyHeader()
			    .AllowAnyMethod()
			    .AllowCredentials();

		    });
	});
builder.Services.AddSignalR();
var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
app.MapGet("/", () => "Hello World!");
app.MapHub<ChatHub>("/chat");
app.Run();
