using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class RegistrationRequest
    {
        [JsonProperty("kind")]
        public string Kind => "user";

        [JsonProperty("auth")]
        public AuthenticationData AuthenticationData { get; set; }

        [JsonProperty("username")] 
        public string Username { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
        
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("initial_device_display_name")]
        public string InitialDeviceDisplayName { get; set; }
        
        [JsonProperty("inhibit_login")]
        public bool InhibitLogin { get; set; }
    }
}