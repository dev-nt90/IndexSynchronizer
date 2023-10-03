using IndexSynchronizer.Models;

namespace IndexSynchronizer.Services
{
	public interface IIndexSyncService
	{
		public Task StartAsync(IConnectionDetails source, IConnectionDetails target, String operationIdentifier);
		public Task StopAsync(String operationIdentifier);
	}
}
