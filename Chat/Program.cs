var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => 
	{
	    o.AddPolicy("AllowSpecificOrigin",
		    builder => 
		    {
			builder.WithOrigins("https://localhost:7222")
			    .AllowAnyHeader()
			    .AllowAnyMethod()
			    .AllowCredentials();

		    });
	});

builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");
app.MapHub<ChatHub>("/chat");
app.Run();
