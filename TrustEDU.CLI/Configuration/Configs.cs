using Microsoft.Extensions.Configuration;

namespace TrustEDU.CLI.Configuration
{
    internal class Configs
    {
        public PathConfigs Paths { get; }
        public P2PConfigs P2P { get; }
        public RPCConfigs RPC { get; }
        public UnlockWalletConfigs UnlockWallet { get; set; }

        public static Configs Default { get; }

        static Configs()
        {
            IConfigurationSection section =
                new ConfigurationBuilder()
                    .AddJsonFile("config.json")
                    .Build()
                    .GetSection("ApplicationConfiguration");
            Default = new Configs(section);
        }

        public Configs(IConfigurationSection section)
        {
            this.Paths = new PathConfigs(section.GetSection("Paths"));
            this.P2P = new P2PConfigs(section.GetSection("P2P"));
            this.RPC = new RPCConfigs(section.GetSection("RPC"));
            this.UnlockWallet = new UnlockWalletConfigs(section.GetSection("UnlockWallet"));
        }
    }
}