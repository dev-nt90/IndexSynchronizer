using IndexSynchronizerServices.Models;

namespace IndexSynchronizerServices.Repositories
{
    public interface IIndexSyncRepository
    {
        Task DoIndexSync(IDictionary<String, String> sourceIndexDefinitions, IConnectionDetails target);
    }
}
