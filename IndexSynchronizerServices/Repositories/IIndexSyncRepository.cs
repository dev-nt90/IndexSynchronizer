using IndexSynchronizerServices.Models;

namespace IndexSynchronizerServices.Repositories
{
	public interface IIndexSyncRepository
	{
		Task DoIndexSync(IEnumerable<String> sourceIndexDefinitions, IConnectionDetails target);
	}
}
