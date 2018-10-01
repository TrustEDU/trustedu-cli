using Microsoft.Extensions.Configuration;

namespace TrustEDU.CLI.Configuration
{
    internal class RPCConfigs
    {
        public ushort Port { get; }
        public string SslCert { get; }
        public string SslCertPassword { get; }

        public RPCConfigs(IConfigurationSection section)
        {
            this.Port = ushort.Parse(section.GetSection("Port").Value);
            this.SslCert = section.GetSection("SslCert").Value;
            this.SslCertPassword = section.GetSection("SslCertPassword").Value;
        }
    }
}