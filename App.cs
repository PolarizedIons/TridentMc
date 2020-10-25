using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using TridentMc.TridentMc;

namespace TridentMc
{
    public class App : IHostedService
    {
        private readonly TridentMcService _tridentMcService;

        public App(TridentMcService tridentMcService)
        {
            _tridentMcService = tridentMcService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Starting Server...");
            await _tridentMcService.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Shutting down Server...");
            await _tridentMcService.Stop();
        }
    }
}
