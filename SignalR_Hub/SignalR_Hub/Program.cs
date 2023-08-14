using SignalRWebpack.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<TestHub>("/hub/testhub");
app.Run();
