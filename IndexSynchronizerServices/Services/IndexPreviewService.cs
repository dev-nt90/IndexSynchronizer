using IndexSynchronizerServices.Models;
using IndexSynchronizerServices.Repositories;

namespace IndexSynchronizerServices.Services
{
	public class IndexPreviewService : IIndexPreviewService
	{
		private readonly IIndexDefinitionRepository indexPreviewRepository;

		public IndexPreviewService(IIndexDefinitionRepository indexPreviewRepository)
		{
			this.indexPreviewRepository = indexPreviewRepository;
		}

		public async Task<IEnumerable<String>> GetIndexDefinitionsAsync(ConnectionDetails connectionDetails)
		{
			if (connectionDetails == null)
			{
				throw new ArgumentNullException(nameof(connectionDetails));
			}

			return await this.indexPreviewRepository.GetIndexDefinitionsAsync(connectionDetails).ConfigureAwait(false);
		}
	}
}
