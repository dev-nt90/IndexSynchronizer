using IndexSynchronizer.Models;
using IndexSynchronizer.Repositories;
using Microsoft.Owin.Security.Provider;

namespace IndexSynchronizer.Services
{
	public class IndexPreviewService : IIndexPreviewService
	{
		private readonly IIndexDefinitionRepository indexPreviewRepository;

		public IndexPreviewService(IIndexDefinitionRepository indexPreviewRepository)
		{
			this.indexPreviewRepository = indexPreviewRepository;
		}

		public async Task<IEnumerable<String>> GetIndexDefinitionsAsync(IConnectionDetails connectionDetails)
		{
			if (connectionDetails == null)
			{
				throw new ArgumentNullException(nameof(connectionDetails));
			}

			return await this.indexPreviewRepository.GetIndexDefinitionsAsync(connectionDetails).ConfigureAwait(false);
		}
	}
}
