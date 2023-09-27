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

		public async Task<String> PreviewRequestSource(ConnectionDetails details)
		{
			try
			{
				var data = String.Empty;

				return String.Empty;
			}
			catch (Exception ex) 
			{
				this.logger.LogError(ex, "An unexpected exception occurred while requesting the source preview");
				throw;
			}

			return String.Empty;
		}

		public async Task<String> PreviewRequestTarget(ConnectionDetails details)
		{
			try
			{
				var data = String.Empty;

				return String.Empty;
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, "An unexpected exception occurred while requesting the target preview");
				throw;
			}

			return String.Empty;
		}
	}
}
