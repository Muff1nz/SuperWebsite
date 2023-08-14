using Microsoft.AspNetCore.SignalR;

namespace SignalRWebpack.Hubs;

public class TestHub : Hub
{
    public async Task NewMessage(long username, string message) =>
        await Clients.All.SendAsync("messageReceived", username, message);
}