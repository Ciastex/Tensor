using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class RegistrationResult
    {
        [JsonProperty("user_id")]
        public string UserId { get; protected set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; protected set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; protected set; }
    }
}