using Microsoft.AspNetCore.SignalR;

namespace IndexSynchronizer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, Context.ConnectionId);
        }
    }
}
