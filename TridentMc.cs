using System;
using System.Threading;
using Serilog;
using TridentMc.Events;
using TridentMc.Networking;

namespace TridentMc
{
    class TridentMc
    {
        public const int ProtocolVersion = 578;
        public const string ProtocolName = "1.15.2";

        private static TridentMc _instance;
        public static TridentMc Instance
        {
            get
            {
                if (_instance == null)
                {
                    Log.Information("Initializing TridentMc...");
                    _instance = new TridentMc();
                }

                return _instance;
            }
        }

        public TridentState State { get; private set; }
        
        public NetworkManager NetworkManager { get; private set; }
        
        public EventManager EventManager { get; private set; }
        
        private TridentMc()
        {
            State = new TridentState();
            NetworkManager = new NetworkManager();
            EventManager = new EventManager();
        }

        public void Start()
        {
            Log.Information("Starting server");
            NetworkManager.Start();
            MainLoop();
        }

        public void Shutdown()
        {
            State.IsShuttingDown = true;
            NetworkManager.Stop();
        }

        private void MainLoop()
        {
            while (!State.IsShuttingDown)
            {
                EventManager.Tick();
                Thread.Sleep(50);
                EventManager.Tock();
            }
        }
    }
}
