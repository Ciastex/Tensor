using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class LoginRequest
    {
        public virtual string Type { get; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("initial_device_display_name")]
        public string InitialDeviceDisplayName { get; set; }
    }
}