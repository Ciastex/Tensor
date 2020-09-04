using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tensor.Matrix.Client
{
    public sealed class ProtocolVersions
    {
        [JsonProperty("versions", Required = Required.Always)]
        public List<string> Versions { get; protected set; }

        [JsonProperty("unstable_features")]
        public Dictionary<string, bool> UnstableFeatures { get; protected set; }
    }
}