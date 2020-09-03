using System;
using System.Collections.Generic;
using RestSharp;

namespace Tensor.Rest
{
    internal sealed class Post : Request
    {
        private Post(string endpoint) : base(endpoint, Method.POST) { }
        
        public static Post To(string endpoint)
            => new Post(endpoint);
    }
}