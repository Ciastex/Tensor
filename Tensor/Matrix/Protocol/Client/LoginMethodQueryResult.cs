using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public class LoginMethodQueryResult
    {
        [JsonProperty("flows", Required = Required.Always)]
        public List<LoginMethod> SupportedMethods { get; protected set; } = new List<LoginMethod>();
    }
}