using IndexSynchronizerServices.Models;
using IndexSynchronizerServices.Repositories;

using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;

namespace IndexSynchronizerServices.Services
{
    public class IndexSynchronizerService : IIndexSyncService
    {
        private readonly ILogger logger;
        private readonly IIndexDefinitionRepository indexDefinitionRepository;
		private readonly IIndexSyncRepository indexSyncRepository;
		private ConcurrentDictionary<String, CancellationTokenSource> runningOperations = new ConcurrentDictionary<String, CancellationTokenSource>();

		public IndexSynchronizerService(
			ILogger logger, 
			IIndexDefinitionRepository indexDefinitionRepository,
			IIndexSyncRepository indexSyncRepository)
		{
			this.logger = logger;
			this.indexDefinitionRepository = indexDefinitionRepository;
			this.indexSyncRepository = indexSyncRepository;
		}

		/// <summary>
		/// Take connection details from <paramref name="source"/> and apply to <paramref name="target"/>. 
		/// Also record this task so it can cancelled in the future.
		/// </summary>
		/// <param name="source">The source connection details</param>
		/// <param name="target">The target connection details</param>
		/// <param name="operationIdentifier">The operation identifier used to track this operation</param>
		public async Task StartAsync(IConnectionDetails source, IConnectionDetails target, String operationIdentifier)
		{
			if (source == null) 
			{
				throw new ArgumentNullException("Invalid connection details for Source"); 
			}

			if (target == null)
			{
				throw new ArgumentNullException("Invalid connection details for Target");
			}

			if (String.IsNullOrWhiteSpace(operationIdentifier))
			{
				throw new ArgumentNullException("Invalid operationIdentifier");
			}

			var tokenSource = new CancellationTokenSource();
			if(!runningOperations.TryAdd(operationIdentifier, tokenSource))
			{
				throw new InvalidOperationException($"Could not add a new operation to {this.GetType().Name}");
			}
			
			await SyncIndexesAsync(source, target, operationIdentifier, tokenSource.Token);
		}

		public async Task StopAsync(String operationIdentifier)
		{
			try
			{
				this.runningOperations[operationIdentifier].Cancel();
			}
			catch (Exception ex)
			{
				this.logger.LogError($"Something went wrong while trying to cancel the task with operation identifier: {operationIdentifier}", ex);
				throw;
			}
		}

		private async Task SyncIndexesAsync(IConnectionDetails source, IConnectionDetails target, String operationIdentifier, CancellationToken cancellationToken)
		{
			try
			{
				var sourceIndexDefinitions = await this.indexDefinitionRepository.GetIndexDefinitionsAsync(source);
				Task transactionTask = null;

				do
				{
					if (transactionTask == null)
					{
						transactionTask = this.indexSyncRepository.DoIndexSync(sourceIndexDefinitions, target);
					}

					await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
				}
				while (!this.runningOperations[operationIdentifier].IsCancellationRequested && !transactionTask.IsCompleted);
			}
			catch (Exception ex)
			{
				logger.LogError("An unexpected exception occurred while synchronizing indexes", ex);
				throw;
			}
			finally
			{
				this.runningOperations.TryRemove(operationIdentifier, out _);
			}
		}
	}
}
