using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class AuthenticationData
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("session")]
        public string SessionKey { get; set; }
    }
}