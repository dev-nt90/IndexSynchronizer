using IndexSynchronizerServices.Models;
using IndexSynchronizerServices.Repositories;

namespace IndexSynchronizerServices.Services
{
    public class IndexDefinitionService : IIndexDefinitionService
    {
        private readonly IIndexDefinitionRepository indexPreviewRepository;

        public IndexDefinitionService(IIndexDefinitionRepository indexPreviewRepository)
        {
            this.indexPreviewRepository = indexPreviewRepository;
        }

        public async Task<IDictionary<String, String>> GetIndexDefinitionsAsync(ConnectionDetails connectionDetails)
        {
            if (connectionDetails == null)
            {
                throw new ArgumentNullException(nameof(connectionDetails));
            }

            return await this.indexPreviewRepository.GetIndexDefinitionsAsync(connectionDetails).ConfigureAwait(false);
        }
    }
}
