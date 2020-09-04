using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class Identifier
    {
        [JsonProperty("type")]
        public virtual string Type { get; }
    }
}