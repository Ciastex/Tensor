using System;
using System.Collections.Generic;
using RestSharp;

namespace Tensor.Rest
{
    internal sealed class Get : Request
    {
        private Get(string endpoint) : base(endpoint, Method.GET) { }
        
        public static Get To(string endpoint)
            => new Get(endpoint);
    }
}