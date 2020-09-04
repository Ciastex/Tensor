using Newtonsoft.Json;

namespace Tensor.Matrix.Protocol
{
    public sealed class ServerInformation
    {
        [JsonProperty("base_url")]
        public string BaseUrl { get; protected set; }
    }
}