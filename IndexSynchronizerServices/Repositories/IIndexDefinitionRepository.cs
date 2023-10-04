using IndexSynchronizerServices.Models;

namespace IndexSynchronizerServices.Repositories
{
	public interface IIndexDefinitionRepository
	{
		public Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails);
	}
}
