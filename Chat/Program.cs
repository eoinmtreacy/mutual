var builder = WebApplication.CreateBuilder(args);

string MyAllowedOrigins = "myAllowedOrigins";

builder.Services.AddCors(o => 
	{
	    o.AddPolicy(MyAllowedOrigins, policy =>
		    {
			policy.WithOrigins("https://localhost:7222");
			policy.WithMethods("GET", "POST");
			policy.AllowCredentials();
		    });
	    });

builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(MyAllowedOrigins);
app.MapHub<ChatHub>("/chat");
app.Run();
