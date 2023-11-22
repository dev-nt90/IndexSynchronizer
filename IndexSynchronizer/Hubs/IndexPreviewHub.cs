using IndexSynchronizerServices.Models;
using IndexSynchronizerServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IndexSynchronizer.Hubs
{
    public class IndexPreviewHub : Hub
    {
        private readonly ILogger logger;
        private readonly IIndexDefinitionService indexPreviewService;

        public IndexPreviewHub(ILogger<IndexPreviewHub> logger, IIndexDefinitionService previewService)
        {
            this.logger = logger;
            this.indexPreviewService = previewService;
        }

        public async Task PreviewRequestSource(ConnectionDetails connectionDetails)
        {
            try
            {
                var indexPreview = await this.indexPreviewService.GetIndexDefinitionsAsync(connectionDetails);
                await Clients.Caller.SendAsync("PreviewResponseSource", indexPreview);
            }
            catch (Exception ex) 
            {
                this.logger.LogError(ex, "An unexpected exception occurred while requesting the source preview");
                throw;
            }
        }

        public async Task PreviewRequestTarget(ConnectionDetails connectionDetails)
        {
            try
            {
                var indexPreview = await this.indexPreviewService.GetIndexDefinitionsAsync(connectionDetails);
                await Clients.Caller.SendAsync("PreviewResponseTarget", indexPreview);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An unexpected exception occurred while requesting the target preview");
                throw;
            }
        }
    }
}
