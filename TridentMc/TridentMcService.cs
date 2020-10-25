using System.Threading;
using System.Threading.Tasks;
using TridentMc.Extentions;
using TridentMc.Networking;

namespace TridentMc.TridentMc
{
    public class TridentMcService : ISingletonDiService
    {
        private readonly NetworkingService _networking;

        public TridentMcService(NetworkingService networking)
        {
            _networking = networking;
        }

        public Task Start(CancellationToken cancellationToken)
        {
            _networking.Start(cancellationToken);
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _networking.Stop();
            return Task.CompletedTask;
        }
    }
}
