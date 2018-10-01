using Microsoft.Extensions.Configuration;

namespace TrustEDU.CLI.Configuration
{
    internal class P2PConfigs
    {
        public ushort Port { get; }
        public ushort WsPort { get; }

        public P2PConfigs(IConfigurationSection section)
        {
            this.Port = ushort.Parse(section.GetSection("Port").Value);
            this.WsPort = ushort.Parse(section.GetSection("WsPort").Value);
        }
    }
}