using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class LoginMethod
    {
        [JsonProperty("type")]
        public string Type { get; protected set; }
    }
}