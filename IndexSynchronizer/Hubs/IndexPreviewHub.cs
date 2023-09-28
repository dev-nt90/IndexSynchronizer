using IndexSynchronizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IndexSynchronizer.Hubs
{
	public class IndexPreviewHub : Hub
	{
		private readonly ILogger logger;
		public IndexPreviewHub(ILogger<IndexPreviewHub> logger)
		{
			this.logger = logger;
		}

		public async Task PreviewRequestSource(ConnectionDetails details)
		{
			try
			{
				await Clients.Caller.SendAsync("PreviewResponseSource", "Preview response source");
			}
			catch (Exception ex) 
			{
				this.logger.LogError(ex, "An unexpected exception occurred while requesting the source preview");
				throw;
			}
		}

		public async Task PreviewRequestTarget(ConnectionDetails details)
		{
			try
			{
				await Clients.Caller.SendAsync("PreviewResponseTarget", "Preview response target");
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, "An unexpected exception occurred while requesting the target preview");
				throw;
			}
		}
	}
}
