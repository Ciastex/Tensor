using Newtonsoft.Json;

namespace Tensor.Matrix.Client.Authentication
{
    public class PhoneIdentifier : Identifier
    {
        [JsonProperty("type")]
        public override string Type => "m.id.phone";

        [JsonProperty("country")]
        public string CountryCode { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}