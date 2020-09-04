using Newtonsoft.Json;

namespace Tensor.Matrix.Client.Authentication
{
    public class ThirdPartyIdentifier : Identifier
    {
        [JsonProperty("type")]
        public override string Type => "m.id.thirdparty";

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}