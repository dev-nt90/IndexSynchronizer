using IndexSynchronizer.Models;

namespace IndexSynchronizer.Repositories
{
	public class IndexSyncRepository : IIndexSyncRepository
	{
		public async Task DoIndexSync(IEnumerable<String> sourceIndexDefinitions, IConnectionDetails target)
		{
			// TODO: managing these as a collection of strings is going to suck - make better
			// TODO: this
		}
	}
}
