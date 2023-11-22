using IndexSynchronizerServices.Models;

namespace IndexSynchronizerServices.Services
{
    public interface IIndexDefinitionService
    {
        public Task<IDictionary<String, String>> GetIndexDefinitionsAsync(ConnectionDetails connectionDetails);
    }
}
