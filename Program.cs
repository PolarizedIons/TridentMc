using System;
using Serilog;

namespace TridentMc
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Setting up logging...");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            Log.Information("Bootstrap...");

            // Set up settings
            TridentSettings.BindHost = "0.0.0.0";
            TridentSettings.BindPort = 25565;
            TridentSettings.Description = "Hello from C#!";
            TridentSettings.MaxPlayers = 20;

            // Let's do this!
            TridentMc.Instance.Start();
            Log.CloseAndFlush();
        }
    }
}
