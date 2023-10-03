using IndexSynchronizer.Models;

namespace IndexSynchronizer.Repositories
{
	public interface IIndexSyncRepository
	{
		Task DoIndexSync(IEnumerable<String> sourceIndexDefinitions, IConnectionDetails target);
	}
}
