var builder = WebApplication.CreateBuilder(args);
string MyAllowedOrigins = "myAllowedOrigins";
builder.Services.AddSignalR();
builder.Services.AddCors(o => 
	{
	    o.AddPolicy(MyAllowedOrigins, policy =>
		    {
			policy.AllowAnyOrigin();
			policy.AllowAnyHeader();
			policy.AllowAnyMethod();
			policy.AllowCredentials();
		    });
	    });
var app = builder.Build();
app.UseCors(MyAllowedOrigins);
app.MapHub<ChatHub>("/chat");
app.UseHttpsRedirection();
app.Run();
