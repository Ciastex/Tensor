using Newtonsoft.Json;
using Tensor.Matrix.Protocol;

namespace Tensor.Matrix.Client
{
    public sealed class LoginResult
    {
        [JsonProperty("user_id")]
        public string UserId { get; protected set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; protected set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; protected set; }

        [JsonProperty("well_known")]
        public DiscoveryInformation WellKnown { get; protected set; }
    }
}