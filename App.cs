using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using TridentMc.Services;

namespace TridentMc
{
    public class App : IHostedService
    {
        private readonly TridentService _tridentService;

        public App(TridentService tridentService)
        {
            _tridentService = tridentService;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Starting Server...");
            await _tridentService.Start(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Shutting down Server...");
            return Task.CompletedTask;
        }
    }
}
