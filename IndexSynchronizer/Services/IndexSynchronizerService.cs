namespace IndexSynchronizer.Services
{
    public class IndexSynchronizerService : IHostedService
    {
        private CancellationTokenSource tokenSource;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            tokenSource = new CancellationTokenSource();
            
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            tokenSource.Cancel();
            await Task.CompletedTask;
        }

        public async void DoSynchronize()
        {
            // TODO: this
            if (tokenSource.IsCancellationRequested)
            {
                return;
            }
        }
    }
}
