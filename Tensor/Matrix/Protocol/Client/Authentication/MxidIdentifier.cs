using Newtonsoft.Json;

namespace Tensor.Matrix.Client.Authentication
{
    public class MxidIdentifier : Identifier
    {
        [JsonProperty("type")]
        public override string Type => "m.id.user";

        [JsonProperty("user")]
        public string User { get; set; }
    }
}