using IndexSynchronizer.Models;

namespace IndexSynchronizer.Repositories
{
	public interface IIndexDefinitionRepository
	{
		public Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails);
	}
}
