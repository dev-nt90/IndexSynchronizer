using IndexSynchronizer.Models;

namespace IndexSynchronizer.Services
{
	public interface IIndexPreviewService
	{
		public Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails);
	}
}
