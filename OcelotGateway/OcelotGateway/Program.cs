using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("configuration.json");
builder.Services.AddSingleton(builder);
builder.Services.AddOcelot(builder.Configuration);
builder.Logging.AddConsole();

var app = builder.Build();

app.UseWebSockets();
app.UseOcelot().Wait(); 
app.Run();
