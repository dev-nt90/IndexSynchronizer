using IndexSynchronizerServices.Models;

namespace IndexSynchronizerServices.Services
{
	public interface IIndexPreviewService
	{
		public Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails);
	}
}
