using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class PasswordLoginRequest<T> : LoginRequest where T : Identifier
    {
        [JsonProperty("type")]
        public override string Type => "m.login.password";

        [JsonProperty("identifier")]
        public T Identifier { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}