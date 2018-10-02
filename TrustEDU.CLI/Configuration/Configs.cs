using System;
using Microsoft.Extensions.Configuration;

namespace TrustEDU.CLI.Configuration
{
    internal class Configs
    {
        private static readonly Lazy<Configs> _configs = new Lazy<Configs>(() => new Configs());
        public PathConfigs Paths { get; }
        public P2PConfigs P2P { get; }
        public RPCConfigs RPC { get; }
        public UnlockWalletConfigs UnlockWallet { get; set; }

        public static Configs Default => _configs.Value;

        public Configs()
        {
            var section =
                new ConfigurationBuilder()
                    .AddJsonFile("config.json")
                    .Build()
                    .GetSection("Application");

            this.Paths = new PathConfigs(section.GetSection("Paths"));
            this.P2P = new P2PConfigs(section.GetSection("P2P"));
            this.RPC = new RPCConfigs(section.GetSection("RPC"));
            this.UnlockWallet = new UnlockWalletConfigs(section.GetSection("UnlockWallet"));
        }
    }
}