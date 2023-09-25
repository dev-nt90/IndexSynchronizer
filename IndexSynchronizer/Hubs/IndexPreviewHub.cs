using IndexSynchronizer.Models;
using Microsoft.AspNetCore.SignalR;

namespace IndexSynchronizer.Hubs
{
	public class IndexPreviewHub : Hub
	{
		public async Task PreviewRequest(ConnectionDetails details)
		{
			var data = String.Empty;
			// TODO: fire to preview service

			if (details.IsSourceDatabase)
			{
				await Clients.Caller.SendAsync("PreviewResponseSource", data);
			}
			else
			{
				await Clients.Caller.SendAsync("PreviewResponseTarget", data);
			}
		}
	}
}
