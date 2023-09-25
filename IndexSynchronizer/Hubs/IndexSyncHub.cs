using IndexSynchronizer.Models;
using Microsoft.AspNetCore.SignalR;

namespace IndexSynchronizer.Hubs
{
    public class IndexSyncHub : Hub
    {
        public async Task SyncRequest(ConnectionDetails serverA, ConnectionDetails serverB)
        {
            // TODO: fire to sync service
            // TODO: bubble up pass/fail
            await Clients.Caller.SendAsync("SyncResponse");
        }
    }
}
