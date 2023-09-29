using IndexSynchronizer.Models;

namespace IndexSynchronizer.Repositories
{
	public interface IIndexPreviewRepository
	{
		public Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails);
	}
}
