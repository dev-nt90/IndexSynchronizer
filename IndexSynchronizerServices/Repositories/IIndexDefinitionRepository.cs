using IndexSynchronizerServices.Models;

namespace IndexSynchronizerServices.Repositories
{
	public interface IIndexDefinitionRepository
	{
		public Task<IDictionary<String, String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails);
	}
}
