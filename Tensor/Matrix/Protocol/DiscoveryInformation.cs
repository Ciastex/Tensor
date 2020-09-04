using Newtonsoft.Json;

namespace Tensor.Matrix.Protocol
{
    public sealed class DiscoveryInformation
    {
        [JsonProperty("m.homeserver", Required = Required.Always)]
        public ServerInformation HomeServer { get; protected set; }

        [JsonProperty("m.identity_server")]
        public ServerInformation IdentityServer { get; protected set; }
    }
}