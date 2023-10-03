using IndexSynchronizer.Models;
using IndexSynchronizer.Services;
using Microsoft.AspNetCore.SignalR;

namespace IndexSynchronizer.Hubs
{
    public class IndexSyncHub : Hub
    {
        private readonly ILogger logger;
        private readonly IIndexSyncService indexSyncService;

		public IndexSyncHub(ILogger logger, IIndexSyncService indexSyncService)
		{
			this.logger = logger;
			this.indexSyncService = indexSyncService;
		}

		public async Task SyncIndexesAsync(IConnectionDetails source, IConnectionDetails target, String operationIdentifier)
        {
            try
            {
                await this.indexSyncService.StartAsync(source, target, operationIdentifier);

				// TODO: maybe add some metadata (e.g. completion time) to the response? maybe auto-fire an update to a Stats database?
				await Clients.Caller.SendAsync("SyncResponse");
			}
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An unexpected exception occurred while synchronizing indexes");
                throw;
			}
        }
    }
}
