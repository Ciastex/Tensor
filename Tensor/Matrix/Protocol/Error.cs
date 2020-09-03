using Newtonsoft.Json;

namespace Tensor.Matrix.Protocol
{
    public class Error
    {
        [JsonProperty("errcode", Required = Required.Always)]
        public string Code { get; protected set; }
        
        [JsonProperty("error", Required = Required.Always)]
        public string Message { get; protected set; }
    }
}