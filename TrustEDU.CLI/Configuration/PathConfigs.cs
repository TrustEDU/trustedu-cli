using Microsoft.Extensions.Configuration;
using Message = TrustEDU.Core.Network.Peer2Peer.Message;

namespace TrustEDU.CLI.Configuration
{
    internal class PathConfigs
    {
        public string Chain { get; }
        public string Index { get; }

        public PathConfigs(IConfigurationSection section)
        {
            this.Chain = string.Format(section.GetSection("Chain").Value, Message.Magic.ToString("X8"));
            this.Index = string.Format(section.GetSection("Index").Value, Message.Magic.ToString("X8"));
        }
    }
}